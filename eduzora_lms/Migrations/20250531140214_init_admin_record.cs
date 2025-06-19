using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduzora_lms.Migrations
{
    /// <inheritdoc />
    public partial class init_admin_record : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "868CC020-1257-4C2B-8F43-8AE013F4AFC4", "868CC020-1257-4C2B-8F43-8AE013F4AFC4", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A", 0, "aad62400-ce08-467d-aa18-a0f5a99f9e1d", "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEF8tvzdK0N+/JK0zkGbUr04si8ysvKnVgwRb+1AiM3GD4Rq1IoyhfiHrwuVOU8JKIQ==", null, false, "3177b0e7-8bfa-4668-b77e-836084e3b79e", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "868CC020-1257-4C2B-8F43-8AE013F4AFC4", "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "868CC020-1257-4C2B-8F43-8AE013F4AFC4", "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "868CC020-1257-4C2B-8F43-8AE013F4AFC4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A");
        }
    }
}
