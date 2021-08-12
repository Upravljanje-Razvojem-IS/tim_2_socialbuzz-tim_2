﻿// <auto-generated />
using System;
using BlockService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlockService.Migrations
{
    [DbContext(typeof(ContextDB))]
    [Migration("20210811121848_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BlockService.Entities.Block", b =>
                {
                    b.Property<Guid>("BlockID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BlockDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("blockedID")
                        .HasColumnType("int");

                    b.Property<int>("blockerID")
                        .HasColumnType("int");

                    b.HasKey("BlockID");

                    b.ToTable("Block");

                    b.HasData(
                        new
                        {
                            BlockID = new Guid("8ca02e0f-a565-43d7-b8d1-da0a073118fb"),
                            BlockDate = new DateTime(2009, 6, 3, 5, 30, 0, 0, DateTimeKind.Unspecified),
                            blockedID = 2,
                            blockerID = 1
                        });
                });
#pragma warning restore 612, 618
        }
    }
}