using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCentralManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDateOfBirthStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonAttended",
                table: "Enrollments",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonAttended",
                table: "Enrollments");
        }
    }
}
