using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Migrations
{
    public partial class seedUserDbContextTables : Migration
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
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporation", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Corporation_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Corporation_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalUser", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_PersonalUser_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalUser_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "CityId", "CityName" },
                values: new object[,]
                {
                    { new Guid("9171f23e-adf2-4698-b73f-05c6fd7ad1be"), "Novi Sad" },
                    { new Guid("9346b8c4-1b3b-435f-9c35-35de3a76fcf9"), "Beograd" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { new Guid("194df880-d4ce-4997-96c9-878102eb6e0e"), "Admin" },
                    { new Guid("728569aa-7a1f-45c9-b9d4-94bcc176bd0c"), "Regular user" }
                });

            migrationBuilder.InsertData(
                table: "Corporation",
                columns: new[] { "UserId", "CityId", "CorporationName", "Email", "HeadquartersAddress", "HeadquartersCity", "IsActive", "Pib", "RoleId", "Telephone", "Username" },
                values: new object[,]
                {
                    { new Guid("33253633-10e4-45c8-9b8e-84020a5c8c58"), new Guid("9171f23e-adf2-4698-b73f-05c6fd7ad1be"), "Financial Corporation", "financial_corpo@gmail.com", "Radnicka 1", "Novi Sad", true, "187398", new Guid("728569aa-7a1f-45c9-b9d4-94bcc176bd0c"), "+3816228749275", "Financial Corporation" },
                    { new Guid("987268e5-f880-4f81-b1bf-5b9704604e26"), new Guid("9171f23e-adf2-4698-b73f-05c6fd7ad1be"), "Billing Corporation", "billing_corpo@gmail.com", "Danila Kisa 15", "Novi Sad", true, "1844398", new Guid("728569aa-7a1f-45c9-b9d4-94bcc176bd0c"), "+3816228749275", "Billing Corporation" }
                });

            migrationBuilder.InsertData(
                table: "PersonalUser",
                columns: new[] { "UserId", "CityId", "Email", "FirstName", "IsActive", "LastName", "RoleId", "Telephone", "Username" },
                values: new object[,]
                {
                    { new Guid("ce593d02-c615-4af6-a794-c450b79e9b4d"), new Guid("9171f23e-adf2-4698-b73f-05c6fd7ad1be"), "nata@gmail.com", "Natalija", true, "Gajic", new Guid("194df880-d4ce-4997-96c9-878102eb6e0e"), "+3816928749275", "NatalijaG" },
                    { new Guid("ff0c9396-7c4c-4bf5-a12e-6aa79c272413"), new Guid("9171f23e-adf2-4698-b73f-05c6fd7ad1be"), "vladika@gmail.com", "Vladimir", true, "Filipovic", new Guid("728569aa-7a1f-45c9-b9d4-94bcc176bd0c"), "+3816968749275", "VladikaF" },
                    { new Guid("8c349e7b-1c97-486d-aa2e-e58205d11577"), new Guid("9346b8c4-1b3b-435f-9c35-35de3a76fcf9"), "stefke@gmail.com", "Stefan", true, "Ostojic", new Guid("728569aa-7a1f-45c9-b9d4-94bcc176bd0c"), "+3816928749275", "StefanO" }
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
                name: "IX_Corporation_Username",
                table: "Corporation",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalUser_CityId",
                table: "PersonalUser",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalUser_RoleId",
                table: "PersonalUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalUser_Username",
                table: "PersonalUser",
                column: "Username",
                unique: true);
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
