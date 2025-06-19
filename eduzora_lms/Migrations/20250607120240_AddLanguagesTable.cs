using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduzora_lms.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff46cda8-f5ff-45e6-ba2e-64b8b7e143dc", "AQAAAAIAAYagAAAAEA6wr62oROI3N+In6aj/3szRQtgNPce17z/Qb3IyTsOgZ4ehWmionh2c64gK+76f3A==", "e06b3af1-fe5f-478a-95a2-e220a676b31d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5abdb409-8e7e-4f0a-aea2-4a5abfc286df", "AQAAAAIAAYagAAAAEOlOQV2wvZd2XDvEjzuT5VnkuPxoukZJN5rdeFGxxoKMC3xbawebtikcuktc1QgBNA==", "3b5a1da3-fc08-4329-b6ee-54679ac84c50" });
        }
    }
}
