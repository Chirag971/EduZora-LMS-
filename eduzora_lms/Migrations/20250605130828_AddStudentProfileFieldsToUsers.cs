using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduzora_lms.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentProfileFieldsToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zipcode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "Address", "City", "ConcurrencyStamp", "Country", "PasswordHash", "SecurityStamp", "State", "Zipcode" },
                values: new object[] { null, null, "59932996-2efc-4d4f-9dbe-b4d17e878e7c", null, "AQAAAAIAAYagAAAAECwmVd/Qmpn9BnHTg8+JOlrM5lrcnEk/R6ui8hWbF2F1/Jr71KTtk1awvwilQ1XAAQ==", "b3fb77ad-0d19-41c1-9bbf-58c1855aa4f9", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Zipcode",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c943ca90-31e7-40c1-9b2e-c69e7eb6963b", "AQAAAAIAAYagAAAAEB3pd3Oc3MmOrZwG81wELdggphb6+ITbu9xhMRGtkczycstYwyc5i13MqTegRsGFZg==", "98f50d61-365d-4bcc-8238-40e65f530de5" });
        }
    }
}
