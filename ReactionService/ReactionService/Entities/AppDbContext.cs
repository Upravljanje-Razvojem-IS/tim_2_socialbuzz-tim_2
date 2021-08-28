using Microsoft.EntityFrameworkCore;

namespace ReactionService.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<ReactionType> ReactionTypes { get; set; }
    }
}
