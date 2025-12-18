using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace INFORCE_.NET_TASK.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Abouts_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShortUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalUrl = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    ShortCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortUrls_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("9b1140c9-f733-412c-9f69-71871b264fe5"), new DateTime(2025, 12, 17, 10, 56, 13, 374, DateTimeKind.Utc).AddTicks(2372), "1uWv6J7SXaRaTzeDrE2dlhUR7KkAnVnvwlftKvlM6QE=", 0, "user2" },
                    { new Guid("a2aced89-1aba-4640-87a7-1c195d68d601"), new DateTime(2025, 12, 17, 10, 56, 13, 374, DateTimeKind.Utc).AddTicks(2317), "68/JmqiBiD/ZoGt4tQsUDfZfJ5RHDkRNV0cDRdrNtTY=", 1, "admin1" },
                    { new Guid("a4a12695-8012-4eaf-8c16-f8af078a8cae"), new DateTime(2025, 12, 17, 10, 56, 13, 374, DateTimeKind.Utc).AddTicks(2363), "EmVMAkpMCSYyl1PPedS7ldYXwXaEBmgJz0U7IISk1bw=", 0, "user1" },
                    { new Guid("c26078b7-4037-4215-ba17-aeda9fffd02a"), new DateTime(2025, 12, 17, 10, 56, 13, 374, DateTimeKind.Utc).AddTicks(2339), "gE2i28K51zMbMZmVt4/spWVyqtACQ9sPKxC+ukwiTSk=", 1, "admin2" }
                });

            migrationBuilder.InsertData(
                table: "Abouts",
                columns: new[] { "Id", "Description", "LastUpdated", "Title", "UpdatedByUserId" },
                values: new object[] { 1, "Lorem ipsum", new DateTime(2025, 12, 17, 10, 56, 13, 374, DateTimeKind.Utc).AddTicks(2559), "URL Shortener Algorithm", new Guid("a2aced89-1aba-4640-87a7-1c195d68d601") });

            migrationBuilder.CreateIndex(
                name: "IX_Abouts_UpdatedByUserId",
                table: "Abouts",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_CreatedByUserId",
                table: "ShortUrls",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_OriginalUrl",
                table: "ShortUrls",
                column: "OriginalUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_ShortCode",
                table: "ShortUrls",
                column: "ShortCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abouts");

            migrationBuilder.DropTable(
                name: "ShortUrls");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
