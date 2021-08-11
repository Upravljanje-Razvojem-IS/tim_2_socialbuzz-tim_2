using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Block",
                columns: table => new
                {
                    BlockID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlockDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    blockerID = table.Column<int>(type: "int", nullable: false),
                    blockedID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Block", x => x.BlockID);
                });

            migrationBuilder.InsertData(
                table: "Block",
                columns: new[] { "BlockID", "BlockDate", "blockedID", "blockerID" },
                values: new object[] { new Guid("8ca02e0f-a565-43d7-b8d1-da0a073118fb"), new DateTime(2009, 6, 3, 5, 30, 0, 0, DateTimeKind.Unspecified), 2, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Block");
        }
    }
}
