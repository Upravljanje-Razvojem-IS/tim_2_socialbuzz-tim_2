using ImageMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(t => t.NoteID);

            builder.Property(t => t.NoteID)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.NoteCreated)
                .IsRequired(false)
                .HasDefaultValueSql("getdate()");

            builder.Property(t => t.NoteLastModified)
             .IsRequired(false)
             .HasDefaultValueSql("getdate()")
             .ValueGeneratedOnAddOrUpdate();

            builder.HasOne(t => t.User)
                .WithMany(t => t.Notes)
                .HasForeignKey(t => t.UserID)
                .IsRequired()
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);

        }
    }
}
