﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RatingService.Entities;

namespace RatingService.Migrations
{
    [DbContext(typeof(ContextDB))]
    partial class ContextDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RatingService.Entities.Rating", b =>
                {
                    b.Property<Guid>("RatingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PostID")
                        .HasColumnType("int");

                    b.Property<DateTime>("RatingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RatingDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RatingTypeID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("RatingID");

                    b.ToTable("Rating");

                    b.HasData(
                        new
                        {
                            RatingID = new Guid("8ca02e0f-a565-43d7-b8d1-da0a073118fb"),
                            PostID = 1,
                            RatingDate = new DateTime(2009, 6, 3, 5, 30, 0, 0, DateTimeKind.Unspecified),
                            RatingDescription = "some description",
                            RatingTypeID = 1,
                            UserID = 1
                        });
                });

            modelBuilder.Entity("RatingService.Entities.RatingType", b =>
                {
                    b.Property<int>("RatingTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RatingTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RatingTypeID");

                    b.ToTable("RatingType");
                });
#pragma warning restore 612, 618
        }
    }
}
