using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UserService.Entities
{
    public class UserDbContext: DbContext
    {
        private readonly IConfiguration configuration;
        public UserDbContext(DbContextOptions<UserDbContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("UserDB"));
           
        }

        public DbSet<PersonalUser> PersonalUser { get; set; }
        public DbSet<Corporation> Corporation { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Role> Role { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PersonalUser>()
            .HasIndex(b => b.Username)
            .IsUnique();
            modelBuilder.Entity<Corporation>()
            .HasIndex(b => b.Username)
            .IsUnique();
            //TODO: email unique
            modelBuilder.Seed();

        }
    }
}
