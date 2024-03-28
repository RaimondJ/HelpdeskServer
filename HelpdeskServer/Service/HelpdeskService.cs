using HelpdeskServer.Data;
using HelpdeskServer.DTO;
using HelpdeskServer.Models;

namespace HelpdeskServer.Service;

public class HelpdeskService(PostRepository repository)
{
    readonly PostRepository _repository = repository;
    public async Task addPost(DTO.Request.PostDto post)
    {
        await _repository.AddAsync(new Post
        {
            beginDate = DateTime.Now,
            description = post.description,
            endDate = post.endDate,
            subject = post.subject
        });
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

    public async Task<Post> getPostById(int id)
    {
        return await _repository.GetAsync(id);
    }

    public async Task<int> getAllPostsCount()
    {
        return await _repository.GetPostsCountAsync();
    }

    public async Task<bool> updateIsClosedAsync(int id, bool isClosed)
    {
        return await _repository.UpdateIsClosedAsync(id, isClosed);
    }
}