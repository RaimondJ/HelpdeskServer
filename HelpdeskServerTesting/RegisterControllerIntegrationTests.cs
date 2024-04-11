using Azure.Core.GeoJson;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;
using static HelpdeskServerTesting.Global.Global;

namespace HelpdeskServerTesting
{
    public class RegisterControllerIntegrationTests : IClassFixture<TestingHelpdeskFactory<Program>>
    {
        private readonly HttpClient _client;
        private const string POST_EXPIRED_DEADLINE_FAILURE_MESSAGE = "Pöördumise tähtaeg ei saa olla juba möödunud aeg.";
        private const string POST_EXPIRED_DISPLAY_MESSAGE = "Tähtaja ületanud!";
        private const string GET_ALL_POSTS_API_END_POINT = "/api/helpdesk/posts";
        private const string POST_POST_API_END_POINT = "/api/helpdesk/post";
        private const string DELETE_POST_API_END_POINT = "/api/helpdesk/post/"; //{id}

        public RegisterControllerIntegrationTests(TestingHelpdeskFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        
        public async Task<(int id, string timeLeft, bool isExpiring)> getPostIdByDescription(string description) {
            var allPostsResponse = await _client.GetAsync(GET_ALL_POSTS_API_END_POINT);
            var responseString = await allPostsResponse.Content.ReadAsStringAsync();
            JArray responseJson = JArray.Parse(responseString);
            int postId = -1;
            string timeLeft = "";
            bool isExpiring = false;
            foreach (var post in responseJson) 
            {
                JObject firstPost = post.ToObject<JObject>();
                if (((string)firstPost["description"]).Equals(description)) {
                    postId = (int)firstPost["id"];
                    timeLeft = (string)firstPost["timeLeft"];
                    isExpiring = (bool)firstPost["isExpiring"];
                }
            }
            return (postId, timeLeft, isExpiring);
        }

        public async Task createPostWithOptions(string subject, string description, DateTime endDate) {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, POST_POST_API_END_POINT);
            string json = Global.Global.BuildJson(
                new List<(string, object)>
                {
                    ("subject", subject),
                    ("description", description),
                    ("endDate", endDate)
                }
            );
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            postRequest.Content = content;
            var response = await _client.SendAsync(postRequest);
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException ex) {
                Assert.Fail(ex.Message);
            }
        }
    

        /*
            Make a post request to create a new post, but with already expired deadline date
            Post request should not succeed
         */
        [Fact]
        public async Task Create_POST_Action_MakeNewPostWithExpiredDate()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, POST_POST_API_END_POINT);

            //Define expired deadline
            DateTime endDate = DateTime.Now.AddDays(-1);

            string json = BuildJson(
                new List<(string, object)>
                {
                    ("subject", "mysubject"),
                    ("description", "somedescription, hello world"),
                    ("endDate", endDate)
                }
            );

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            postRequest.Content = content;

            var response = await _client.SendAsync(postRequest);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            JObject responseJson = JObject.Parse(responseString);

            Assert.False((bool)responseJson["success"]);

            Assert.Equal((string)responseJson["message"], POST_EXPIRED_DEADLINE_FAILURE_MESSAGE);
        }

        [Fact]
        public async Task Create_POST_Action_MakeNewPostAndEnsureItExists()
        {
            string subject = generateRandomString(10);
            string description = generateRandomString(50);
            DateTime endDate = DateTime.Now.AddDays(10);
            
            await createPostWithOptions(subject, description, endDate);

            var allPostsResponse = await _client.GetAsync(GET_ALL_POSTS_API_END_POINT);
            var responseString = await allPostsResponse.Content.ReadAsStringAsync();
            JArray responseJson = JArray.Parse(responseString);
            JObject firstPost = responseJson[0].ToObject<JObject>();

            Assert.Equal((string)firstPost["subject"], subject);
            Assert.Equal((string)firstPost["description"], description);
        }

        [Fact]
        public async Task Create_POST_And_DELETE_Action_EnsureDeletion() {
            //Make a new post
            string subject = generateRandomString(10);
            string description = generateRandomString(50);
            DateTime endDate = DateTime.Now.AddDays(10);

            await createPostWithOptions(subject, description, endDate);

            //Get the new post's id
            //It shouldn't be -1, which means it was not found
            //It should be found
            var getPost = await getPostIdByDescription(description);
            if (getPost.id == -1) 
            {
                Assert.Fail("Post was not found");
            }

            var delRequest = new HttpRequestMessage(HttpMethod.Delete, DELETE_POST_API_END_POINT + getPost.id);
            var delResponse = await _client.SendAsync(delRequest);
            delResponse.EnsureSuccessStatusCode();

            //Get the same post by same description after using DELETE action
            //It should be -1, because it was deleted, and shouldn't appear in API endpoint
            //where we get all posts
            var getPost2 = await getPostIdByDescription(description);
            if (getPost2.id != -1) 
            {
                Assert.Fail("Post was not deleted");
            }
        }

        [Fact]
        public async Task Create_POST_And_GET_Action_WithSoonExpiringDateAndEnsureCorrectResponse() {
            string subject = generateRandomString(10);
            string description = generateRandomString(50);
            DateTime endDate = DateTime.Now.AddSeconds(1);
            await createPostWithOptions(subject, description, endDate);

            var getPost = await getPostIdByDescription(description);
            string timeLeft = getPost.timeLeft;

            //Post's message should not be expired
            Assert.NotEqual(POST_EXPIRED_DISPLAY_MESSAGE, timeLeft);

            //Let's wait for 3 seconds, until the post expires
            //Because we set post's expiry date as DateTime.Now + 1 second
            await Task.Delay(3000);

            var getPost2 = await getPostIdByDescription(description);
            string timeLeft2 = getPost2.timeLeft;

            //Post's message should show as expired
            Assert.Equal(POST_EXPIRED_DISPLAY_MESSAGE, timeLeft2);
        }

        [Fact]
        public async Task Create_POST_And_GET_Action_WithLessThanOneHourDeadlineShouldBeExpiring() {
            string subject = generateRandomString(10);
            string description = generateRandomString(50);
            DateTime endDate = DateTime.Now.AddMinutes(30);

            string subject2 = generateRandomString(10);
            string description2 = generateRandomString(50);
            DateTime endDate2 = DateTime.Now.AddHours(2);

            //Post that expires in 30 minutes, isExpiring should be true (it will be displayed as red in front)
            await createPostWithOptions(subject, description, endDate);

            //Post that expires in 2 hours, isExpiring should be false (won't be displayed as red in front)
            await createPostWithOptions(subject2, description2, endDate2);

            var getPost = await getPostIdByDescription(description);
            var getPost2 = await getPostIdByDescription(description2);

            Assert.True(getPost.isExpiring);
            Assert.False(getPost2.isExpiring);
        }
    }
}   
