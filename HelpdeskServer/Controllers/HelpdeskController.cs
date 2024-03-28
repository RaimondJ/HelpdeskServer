using HelpdeskServer.Data;
using Microsoft.AspNetCore.Mvc;
using HelpdeskServer.Models;
using HelpdeskServer.DTO;
using HelpdeskServer.Service;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata.Ecma335;

namespace HelpdeskServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpdeskController(HelpdeskService helpdeskService) : Controller
    {
        private readonly HelpdeskService _helpdeskService = helpdeskService;

        [HttpPost]
        [Route("post")]
        public async Task<DTO.Response.AddPostDto> AddPost(DTO.Request.PostDto postDto)
        {
            return await _helpdeskService.addPost(postDto);
        }

        [HttpGet]
        [Route("posts")]
        public async Task<IEnumerable<DTO.Response.PostInfoDto>> GetPosts() 
        {
            return await _helpdeskService.getAllOpenPosts();
        }

        [HttpGet]
        [Route("posts_count")]
        public async Task<DTO.Response.PostsCountDto> GetPostsCount()
        {
            return await _helpdeskService.getAllPostsCount();
        }

        [HttpDelete]
        [Route("post/{id}")]
        public async Task<DTO.Response.DeletePostDto> DeletePost(int id)
        {
            return await _helpdeskService.updateIsClosedAsync(id, true);
        }
    }
}
