using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduzora_lms.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "901bc8ef-975b-4beb-8bee-04a7c112fb5d", "AQAAAAIAAYagAAAAEDYgHklyC+wk+qf6x1YuStFAR8JJgX+nk8598BIPm/Gd4w+MAwfNkQaGQfsx50wS4g==", "55007edd-5f8b-4b8c-8c36-f08f99529d52" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4c359ccc-3f36-4f68-bfe3-92ff6db97c5a", "AQAAAAIAAYagAAAAEHt3yKLhbrft82SJEbSacEb8bGzK9S/ifrZ+QwNmhSosD06CfDLGvOEwR1Yp8xNCrw==", "4ea39460-8836-4601-b66b-6fd8590dbd32" });
        }
    }
}
