using HelpdeskServer.Data;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskServer.Models;
public class StudentRepository(HelpdeskDbContext context)
{
    readonly HelpdeskDbContext _studentContext = context;
    public async Task AddAsync(Post entity)
    {
        _studentContext.posts.Add(entity);
        await _studentContext.SaveChangesAsync();
    }
    public async Task<Post?> GetAsync(int id) => await _studentContext.posts.FindAsync(id);

    public async Task DeleteAsync(Post entity)
    {
        _studentContext.Remove(entity);
        await _studentContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await _studentContext.posts.ToListAsync<Post>();
    }
}