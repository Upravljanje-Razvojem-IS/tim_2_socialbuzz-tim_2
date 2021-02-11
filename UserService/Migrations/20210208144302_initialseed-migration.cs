using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Migrations
{
    public partial class initialseedmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Corporation",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorporationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Pib = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HeadquartersCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HeadquartersAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporation", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Corporation_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Corporation_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonalUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalUser", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_PersonalUser_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalUser_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "CityId", "CityName" },
                values: new object[,]
                {
                    { new Guid("98a5cee0-2a63-405c-887a-5bd623b5b5e3"), "Novi Sad" },
                    { new Guid("a4e47e1c-3f77-4c0f-abb5-234e74d19e65"), "Beograd" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { new Guid("d8994917-94b0-40c8-91f5-dd0f81570369"), "Admin" },
                    { new Guid("1dce00e0-00ff-4434-a989-611dad6a08e4"), "Regular user" }
                });

            migrationBuilder.InsertData(
                table: "Corporation",
                columns: new[] { "UserId", "CityId", "CorporationName", "Email", "HeadquartersAddress", "HeadquartersCity", "IsActive", "Password", "Pib", "RoleId", "Telephone", "Username" },
                values: new object[,]
                {
                    { new Guid("113a283a-f8ff-4f09-9895-ae60b9a0755a"), new Guid("98a5cee0-2a63-405c-887a-5bd623b5b5e3"), "Financial Corporation", "financial_corpo@gmail.com", "Radnicka 1", "Novi Sad", true, "pass123", "187398", new Guid("1dce00e0-00ff-4434-a989-611dad6a08e4"), "+3816228749275", "Financial Corporation" },
                    { new Guid("043bea8a-5b7c-4626-9812-635bdf12e7d7"), new Guid("98a5cee0-2a63-405c-887a-5bd623b5b5e3"), "Billing Corporation", "billing_corpo@gmail.com", "Danila Kisa 15", "Novi Sad", true, "pass123", "1844398", new Guid("1dce00e0-00ff-4434-a989-611dad6a08e4"), "+3816228749275", "Billing Corporation" }
                });

            migrationBuilder.InsertData(
                table: "PersonalUser",
                columns: new[] { "UserId", "CityId", "Email", "FirstName", "IsActive", "LastName", "Password", "RoleId", "Telephone", "Username" },
                values: new object[,]
                {
                    { new Guid("2106c90f-7f5d-45f9-a8bd-2c459d015eba"), new Guid("98a5cee0-2a63-405c-887a-5bd623b5b5e3"), "nata@gmail.com", "Natalija", true, "Gajic", "pass123", new Guid("d8994917-94b0-40c8-91f5-dd0f81570369"), "+3816928749275", "NatalijaG" },
                    { new Guid("aa6c4d37-96cc-4661-a497-5dd9ec03aa25"), new Guid("98a5cee0-2a63-405c-887a-5bd623b5b5e3"), "vladika@gmail.com", "Vladimir", true, "Filipovic", "pass123", new Guid("1dce00e0-00ff-4434-a989-611dad6a08e4"), "+3816968749275", "VladikaF" },
                    { new Guid("ae796ebb-9b2a-4e19-be89-44c93d0d792e"), new Guid("a4e47e1c-3f77-4c0f-abb5-234e74d19e65"), "stefke@gmail.com", "Stefan", true, "Ostojic", "pass123", new Guid("1dce00e0-00ff-4434-a989-611dad6a08e4"), "+3816928749275", "StefanO" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Corporation_CityId",
                table: "Corporation",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporation_RoleId",
                table: "Corporation",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalUser_CityId",
                table: "PersonalUser",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalUser_RoleId",
                table: "PersonalUser",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Corporation");

            migrationBuilder.DropTable(
                name: "PersonalUser");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
