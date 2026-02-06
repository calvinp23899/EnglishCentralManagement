using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCentralManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClassTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxStudents",
                table: "Classes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Classes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Classes",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Classes");

            migrationBuilder.AlterColumn<int>(
                name: "MaxStudents",
                table: "Classes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
