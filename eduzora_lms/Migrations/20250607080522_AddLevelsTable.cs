using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduzora_lms.Migrations
{
    /// <inheritdoc />
    public partial class AddLevelsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LevelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5abdb409-8e7e-4f0a-aea2-4a5abfc286df", "AQAAAAIAAYagAAAAEOlOQV2wvZd2XDvEjzuT5VnkuPxoukZJN5rdeFGxxoKMC3xbawebtikcuktc1QgBNA==", "3b5a1da3-fc08-4329-b6ee-54679ac84c50" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "901bc8ef-975b-4beb-8bee-04a7c112fb5d", "AQAAAAIAAYagAAAAEDYgHklyC+wk+qf6x1YuStFAR8JJgX+nk8598BIPm/Gd4w+MAwfNkQaGQfsx50wS4g==", "55007edd-5f8b-4b8c-8c36-f08f99529d52" });
        }
    }
}
