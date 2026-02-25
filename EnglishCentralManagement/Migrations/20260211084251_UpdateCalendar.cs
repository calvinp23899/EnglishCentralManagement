using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishCentralManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCalendar_Staffs_StaffId",
                table: "EventCalendar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventCalendar",
                table: "EventCalendar");

            migrationBuilder.RenameTable(
                name: "EventCalendar",
                newName: "EventCalendars");

            migrationBuilder.RenameIndex(
                name: "IX_EventCalendar_StaffId",
                table: "EventCalendars",
                newName: "IX_EventCalendars_StaffId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventCalendars",
                table: "EventCalendars",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventCalendars_Staffs_StaffId",
                table: "EventCalendars",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCalendars_Staffs_StaffId",
                table: "EventCalendars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventCalendars",
                table: "EventCalendars");

            migrationBuilder.RenameTable(
                name: "EventCalendars",
                newName: "EventCalendar");

            migrationBuilder.RenameIndex(
                name: "IX_EventCalendars_StaffId",
                table: "EventCalendar",
                newName: "IX_EventCalendar_StaffId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventCalendar",
                table: "EventCalendar",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventCalendar_Staffs_StaffId",
                table: "EventCalendar",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
