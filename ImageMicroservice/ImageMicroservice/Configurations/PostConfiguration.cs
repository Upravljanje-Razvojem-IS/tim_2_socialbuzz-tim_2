using ImageMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(t => t.PostID);

            builder.Property(t => t.PostID)
                .ValueGeneratedOnAdd();

            builder.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
