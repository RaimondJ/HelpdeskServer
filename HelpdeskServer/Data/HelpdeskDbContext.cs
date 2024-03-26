using HelpdeskServer.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskServer.Data
{
    public class HelpdeskDbContext : DbContext
    {
        public HelpdeskDbContext(DbContextOptions<HelpdeskDbContext> options) : base(options)
        {
        }

        public DbSet<Post> posts { get; set; }
    }
}
