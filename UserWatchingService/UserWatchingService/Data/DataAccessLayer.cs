using Microsoft.EntityFrameworkCore;
using UserWatchingService.Entities;

namespace UserWatchingService.Data
{
    public class DataAccessLayer : DbContext
    {

        public DataAccessLayer()
        {

        }

        public DataAccessLayer(DbContextOptions<DataAccessLayer> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Watching>()
                     .HasOne(m => m.Watcher)
                     .WithMany(t => t.WatcherList)
                     .HasForeignKey(m => m.WatcherId)
                     .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Watching>()
                        .HasOne(m => m.Watched)
                        .WithMany(t => t.WatchingList)
                        .HasForeignKey(m => m.WatchedId)
                        .OnDelete(DeleteBehavior.NoAction);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Watching> Watchings { get; set; }
        public virtual DbSet<WatchingType> WatchingTypes { get; set; }
    }
}
