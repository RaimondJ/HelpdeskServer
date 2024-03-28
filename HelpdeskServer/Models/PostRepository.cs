using HelpdeskServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

    public async Task<int> GetPostsCountAsync()
    {
        return await _postContext.posts.CountAsync();
    }

    public async Task<IEnumerable<Post>> GetAllOpenPostsAsync()
    {
        return await _postContext.posts.FromSqlInterpolated($@"SELECT * FROM posts WHERE isClosed = false ORDER BY endDate DESC").ToListAsync();
    }

    public async Task<bool> UpdateIsClosedAsync(int id, bool isClosed)
    {
        var post = await _postContext.posts.FindAsync(id);
        if (post != null)
        {
            post.isClosed = isClosed;
            await _postContext.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }
}