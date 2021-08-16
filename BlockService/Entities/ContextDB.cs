
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Entities
{
    public class ContextDB : DbContext
    {

        private readonly IConfiguration configuration;

        public ContextDB(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Block> Block { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Block>()
                 .HasData(new
                 {
                     BlockID = Guid.Parse("8ca02e0f-a565-43d7-b8d1-da0a073118fb"),
                     BlockDate = new DateTime(2009, 6, 3, 5, 30, 0),
                     blockerID = 1,
                     blockedID = 2

                 });
        }
    }
}
