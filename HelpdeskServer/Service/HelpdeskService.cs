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

    public async Task<IEnumerable<Post>> getAllPosts()
    {
        return await _repository.GetAllOpenPostsAsync();
    }

    public async Task<Post> getPostById(int id)
    {
        return await _repository.GetAsync(id);
    }

    public async Task<int> getAllPostsCount()
    {
        return await _repository.GetPostCountAsync();
    }
}