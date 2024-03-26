using HelpdeskServer.Data;
using Microsoft.AspNetCore.Mvc;
using HelpdeskServer.Models;
using HelpdeskServer.DTO;
using HelpdeskServer.Service;
using Microsoft.Extensions.Hosting;

namespace HelpdeskServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpdeskController(HelpdeskService _helpdeskService) : Controller
    {
        [HttpPost]
        [Route("post")]
        public async Task<IActionResult> AddPost(DTO.Request.PostDto postDto)
        {
            await _helpdeskService.addPost(postDto);
            return Ok();
        }

        [HttpGet]
        [Route("posts")]
        public Task<IEnumerable<Post>> GetPosts() 
        {
            return _helpdeskService.getAllPosts();
        }

        [HttpGet]
        [Route("posts_count")]
        public Task<int> GetPostsCount()
        {
            return _helpdeskService.getAllPostsCount();
        }
    }
}
