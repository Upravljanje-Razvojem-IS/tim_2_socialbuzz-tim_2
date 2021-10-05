﻿// <auto-generated />
using System;
using ImageMicroservice;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImageMicroservice.Migrations
{
    [DbContext(typeof(ImageMicroserviceDbContext))]
    partial class ImageMicroserviceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ImageMicroservice.Models.Image", b =>
                {
                    b.Property<long>("ImageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PostID")
                        .HasColumnType("bigint");

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("ImageID");

                    b.HasIndex("PostID");

                    b.HasIndex("UserID");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("ImageMicroservice.Models.Note", b =>
                {
                    b.Property<long>("NoteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("NoteCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("NoteFavorited")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("NoteLastModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("NoteText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("NoteID");

                    b.HasIndex("UserID");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("ImageMicroservice.Models.Post", b =>
                {
                    b.Property<long>("PostID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("PostID");

                    b.HasIndex("UserID");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("ImageMicroservice.Models.User", b =>
                {
                    b.Property<long>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ImageMicroservice.Models.Image", b =>
                {
                    b.HasOne("ImageMicroservice.Models.Post", "Post")
                        .WithMany("Images")
                        .HasForeignKey("PostID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ImageMicroservice.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ImageMicroservice.Models.Note", b =>
                {
                    b.HasOne("ImageMicroservice.Models.User", "User")
                        .WithMany("Notes")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ImageMicroservice.Models.Post", b =>
                {
                    b.HasOne("ImageMicroservice.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ImageMicroservice.Models.Post", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("ImageMicroservice.Models.User", b =>
                {
                    b.Navigation("Notes");
                });
#pragma warning restore 612, 618
        }
    }
}
