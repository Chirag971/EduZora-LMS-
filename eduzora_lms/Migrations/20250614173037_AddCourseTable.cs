using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduzora_lms.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    PriceOld = table.Column<int>(type: "int", nullable: true),
                    Category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalStudent = table.Column<int>(type: "int", nullable: false),
                    TotalRating = table.Column<int>(type: "int", nullable: false),
                    TotalRatingScore = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<int>(type: "int", nullable: false),
                    FeaturedPhoto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeaturedBanner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeaturedVideoType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeaturedVideoContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalVideoHours = table.Column<int>(type: "int", nullable: false),
                    TotalVideos = table.Column<int>(type: "int", nullable: false),
                    TotalResources = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_AspNetUsers_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Categories_Category_id",
                        column: x => x.Category_id,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Languages_Language_id",
                        column: x => x.Language_id,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Levels_Level_id",
                        column: x => x.Level_id,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4cd90f9f-181c-4ed7-9d70-9123fd9a97c4", "AQAAAAIAAYagAAAAEGnZpxLE81nK5X1REg2q19rjSutJpgNoUv3usNqbunjA3brLLcFuRMR9n/S/hrINDg==", "3964e658-7460-4add-96d6-ce4b81504c77" });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Category_id",
                table: "Courses",
                column: "Category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_InstructorId",
                table: "Courses",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Language_id",
                table: "Courses",
                column: "Language_id");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Level_id",
                table: "Courses",
                column: "Level_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "554a233f-c163-480b-974c-31ff86063003", "AQAAAAIAAYagAAAAEKWPWRNVZF+LwTgdKxhvyU/VZkxjXtdQdFPjxYU6J/ksajQcBdhuJmW77RvfekZEtA==", "c9998c57-aae6-49d3-b77f-a9107c519d6e" });
        }
    }
}
