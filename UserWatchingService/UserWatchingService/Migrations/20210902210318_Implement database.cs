using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserWatchingService.Migrations
{
    public partial class Implementdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WatchingTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Watchings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WatcherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WatchedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Watchings_Users_WatchedId",
                        column: x => x.WatchedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Watchings_Users_WatcherId",
                        column: x => x.WatcherId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Watchings_WatchedId",
                table: "Watchings",
                column: "WatchedId");

            migrationBuilder.CreateIndex(
                name: "IX_Watchings_WatcherId",
                table: "Watchings",
                column: "WatcherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Watchings");

            migrationBuilder.DropTable(
                name: "WatchingTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
