using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RatingService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    RatingID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostID = table.Column<int>(type: "int", nullable: false),
                    RatingTypeID = table.Column<int>(type: "int", nullable: false),
                    RatingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RatingDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.RatingID);
                });

            migrationBuilder.CreateTable(
                name: "RatingType",
                columns: table => new
                {
                    RatingTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatingTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingType", x => x.RatingTypeID);
                });

            migrationBuilder.InsertData(
                table: "Rating",
                columns: new[] { "RatingID", "PostID", "RatingDate", "RatingDescription", "RatingTypeID", "UserID" },
                values: new object[] { new Guid("8ca02e0f-a565-43d7-b8d1-da0a073118fb"), 1, new DateTime(2009, 6, 3, 5, 30, 0, 0, DateTimeKind.Unspecified), "some description", 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "RatingType");
        }
    }
}
