using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduzora_lms.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructorDesignationFieldToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "Designation", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4c359ccc-3f36-4f68-bfe3-92ff6db97c5a", null, "AQAAAAIAAYagAAAAEHt3yKLhbrft82SJEbSacEb8bGzK9S/ifrZ+QwNmhSosD06CfDLGvOEwR1Yp8xNCrw==", "4ea39460-8836-4601-b66b-6fd8590dbd32" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designation",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "59932996-2efc-4d4f-9dbe-b4d17e878e7c", "AQAAAAIAAYagAAAAECwmVd/Qmpn9BnHTg8+JOlrM5lrcnEk/R6ui8hWbF2F1/Jr71KTtk1awvwilQ1XAAQ==", "b3fb77ad-0d19-41c1-9bbf-58c1855aa4f9" });
        }
    }
}
