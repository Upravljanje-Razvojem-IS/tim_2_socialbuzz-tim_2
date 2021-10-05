using ImageMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(t => t.ImageID);

            builder.Property(t => t.ImageID)
                .ValueGeneratedOnAdd();

            builder.HasOne(t => t.Post)
                .WithMany(t => t.Images)
                .HasForeignKey(t => t.PostID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(t => t.User)
               .WithMany()
               .HasForeignKey(t => t.UserID)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
