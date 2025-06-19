using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduzora_lms.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoPathColumnToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PhotoPath", "SecurityStamp" },
                values: new object[] { "c943ca90-31e7-40c1-9b2e-c69e7eb6963b", "AQAAAAIAAYagAAAAEB3pd3Oc3MmOrZwG81wELdggphb6+ITbu9xhMRGtkczycstYwyc5i13MqTegRsGFZg==", null, "98f50d61-365d-4bcc-8238-40e65f530de5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aad62400-ce08-467d-aa18-a0f5a99f9e1d", "AQAAAAIAAYagAAAAEF8tvzdK0N+/JK0zkGbUr04si8ysvKnVgwRb+1AiM3GD4Rq1IoyhfiHrwuVOU8JKIQ==", "3177b0e7-8bfa-4668-b77e-836084e3b79e" });
        }
    }
}
