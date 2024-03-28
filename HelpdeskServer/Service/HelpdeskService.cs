using HelpdeskServer.Data;
using HelpdeskServer.DTO;
using HelpdeskServer.Models;

namespace HelpdeskServer.Service;

public class HelpdeskService(PostRepository repository)
{
    readonly PostRepository _repository = repository;
    public async Task<DTO.Response.AddPostDto> addPost(DTO.Request.PostDto postDto)
    {
        if (postDto.endDate < DateTime.Now)
        {
            return new DTO.Response.AddPostDto
            {
                success = false,
                message = "Pöördumise tähtaeg ei saa olla juba möödunud aeg."
            };
        }
        await _repository.AddAsync(new Post
        {
            beginDate = DateTime.Now,
            description = postDto.description,
            endDate = postDto.endDate,
            subject = postDto.subject
        });
        return new DTO.Response.AddPostDto
        {
            success = true,
            message = $"Pöördumine on lisatud, tähtajaga {postDto.endDate.ToString("dd.MM.yyyy HH:mm:ss")}."
        };
    }

    public async Task<IEnumerable<DTO.Response.PostInfoDto>> getAllOpenPosts()
    {
        List<DTO.Response.PostInfoDto> postInfoDtos = new List<DTO.Response.PostInfoDto>();
        IEnumerable<Post> posts = await _repository.GetAllOpenPostsAsync();
        foreach (var post in posts)
        {
            postInfoDtos.Add(new DTO.Response.PostInfoDto
            {
                id = post.Id,
                beginDate = post.beginDate.ToString("dd.MM.yyyy HH:mm:ss"),
                description = post.description,
                endDate = post.endDate.ToString("dd.MM.yyyy HH:mm:ss"),
                subject = post.subject,
                timeLeft = post.endDate < DateTime.Now ? "Tähtaja ületanud!" : $"Tähtaja lõpuni on {Global.Global.timespanToString(post.endDate - DateTime.Now)}",
                isExpiring = post.endDate < DateTime.Now || (post.endDate - DateTime.Now).TotalHours < 1
            });
        }
        return postInfoDtos;
    }

    public async Task<IEnumerable<Post>> getAllPosts()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<DTO.Response.PostsCountDto> getAllPostsCount()
    {
        int total = await _repository.GetPostsCountAsync();
        return new DTO.Response.PostsCountDto { total = total };
    }

    public async Task<DTO.Response.DeletePostDto> updateIsClosedAsync(int id, bool isClosed)
    {
        bool success = await _repository.UpdateIsClosedAsync(id, isClosed);
        if (success)
        {
            return new DTO.Response.DeletePostDto { success = true, message = "Pöördumine märgiti edukalt lahendatuks." };
        }
        return new DTO.Response.DeletePostDto { success = false, message = "Sellist pöördumist ei leitud. Proovi lehte värskendada." };
    }
}