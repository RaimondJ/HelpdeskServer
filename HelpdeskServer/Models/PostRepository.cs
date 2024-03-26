using HelpdeskServer.Data;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskServer.Models;
public class PostRepository(HelpdeskDbContext context)
{
    readonly HelpdeskDbContext _postContext = context;
    public async Task AddAsync(Post entity)
    {
        _postContext.posts.Add(entity);
        await _postContext.SaveChangesAsync();
    }
    public async Task<Post?> GetAsync(int id) => await _postContext.posts.FindAsync(id);

    public async Task DeleteAsync(Post entity)
    {
        _postContext.Remove(entity);
        await _postContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await _postContext.posts.ToListAsync<Post>();
    }
}