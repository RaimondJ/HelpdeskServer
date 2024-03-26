using HelpdeskServer.Data;
using Microsoft.AspNetCore.Mvc;
using HelpdeskServer.Models;
using HelpdeskServer.DTO;
using HelpdeskServer.Service;

namespace HelpdeskServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpdeskController(PostRepository postRepository) : Controller
    {
        private readonly HelpdeskService _helpdeskService = new HelpdeskService(postRepository);
        
        [HttpPost]
        [Route("post")]
        public IActionResult AddPost(Post post)
        {
            _helpdeskService.addPost(post);
            return Ok();
        }

        [HttpGet]
        [Route("posts")]
        public Task<IEnumerable<Post>> GetPosts() 
        {
            return postRepository.GetAllAsync();
        }
    }
}
