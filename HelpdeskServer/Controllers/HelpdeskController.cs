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
        public async Task<DTO.Response.AddPostDto> AddPost(DTO.Request.PostDto postDto)
        {
            if (postDto.endDate < DateTime.Now) 
            {
                return new DTO.Response.AddPostDto
                {
                    success = false,
                    message = "Pöördumise tähtaeg ei saa olla juba möödunud aeg."
                };
            }
            await _helpdeskService.addPost(postDto);
            return new DTO.Response.AddPostDto 
            { 
                success = true, 
                message = $"Pöördumine on lisatud, tähtajaga {postDto.endDate.ToString("dd.MM.yyyy HH:mm:ss")}."
            };
        }

        [HttpGet]
        [Route("posts")]
        public Task<IEnumerable<DTO.Response.PostInfoDto>> GetPosts() 
        {
            return _helpdeskService.getAllOpenPosts();
        }

        [HttpGet]
        [Route("posts_count")]
        public async Task<DTO.Response.PostsCountDto> GetPostsCount()
        {
            int total = await _helpdeskService.getAllPostsCount();
            return new DTO.Response.PostsCountDto { total = total };
        }

        [HttpDelete]
        [Route("post/{id}")]
        public async Task<DTO.Response.DeletePostDto> DeletePost(int id)
        {
            bool success = await _helpdeskService.updateIsClosedAsync(id, true);
            if (success) 
            {
                return new DTO.Response.DeletePostDto { success = true, message = "Pöördumine märgiti edukalt lahendatuks." };
            }
            return new DTO.Response.DeletePostDto { success = false, message = "Sellist pöördumist ei leitud. Proovi lehte värskendada." };
        }
    }
}
