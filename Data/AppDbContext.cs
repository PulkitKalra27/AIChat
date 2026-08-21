using AIChat.Models;
using Microsoft.EntityFrameworkCore;

namespace AIChat.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Conversations> Conversations { get; set; }
        public DbSet<message> message { get; set; }
    }
}
    