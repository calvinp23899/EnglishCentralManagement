using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCentralManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStaffGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "Staffs",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Staffs");
        }
    }
}
