using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduzora_lms.Migrations
{
    /// <inheritdoc />
    public partial class AddSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesCommission = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "554a233f-c163-480b-974c-31ff86063003", "AQAAAAIAAYagAAAAEKWPWRNVZF+LwTgdKxhvyU/VZkxjXtdQdFPjxYU6J/ksajQcBdhuJmW77RvfekZEtA==", "c9998c57-aae6-49d3-b77f-a9107c519d6e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff46cda8-f5ff-45e6-ba2e-64b8b7e143dc", "AQAAAAIAAYagAAAAEA6wr62oROI3N+In6aj/3szRQtgNPce17z/Qb3IyTsOgZ4ehWmionh2c64gK+76f3A==", "e06b3af1-fe5f-478a-95a2-e220a676b31d" });
        }
    }
}
