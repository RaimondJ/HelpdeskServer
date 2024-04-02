using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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


        public RegisterControllerIntegrationTests(TestingHelpdeskFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        /*
            Make a post request to create a new post, but with already expired deadline date
            Post request should not succeed
         */
        [Fact]
        public async Task Create_POST_Action_MakeNewPostWithExpiredDate()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/api/helpdesk/post");

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

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/api/helpdesk/post");

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

            response.EnsureSuccessStatusCode();
           
        }
    }
}
