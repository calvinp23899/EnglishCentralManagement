using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EnglishCentralManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentSchedules_Enrollments_EnrollmentId",
                table: "PaymentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_PaymentSchedules_EnrollmentId",
                table: "PaymentSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_StudentId",
                table: "Enrollments");

            migrationBuilder.AddColumn<long>(
                name: "EnrollmentClassId",
                table: "PaymentSchedules",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "EnrollmentStudentId",
                table: "PaymentSchedules",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Enrollments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments",
                columns: new[] { "StudentId", "ClassId" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedules_EnrollmentStudentId_EnrollmentClassId",
                table: "PaymentSchedules",
                columns: new[] { "EnrollmentStudentId", "EnrollmentClassId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentSchedules_Enrollments_EnrollmentStudentId_Enrollment~",
                table: "PaymentSchedules",
                columns: new[] { "EnrollmentStudentId", "EnrollmentClassId" },
                principalTable: "Enrollments",
                principalColumns: new[] { "StudentId", "ClassId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentSchedules_Enrollments_EnrollmentStudentId_Enrollment~",
                table: "PaymentSchedules");

            migrationBuilder.DropIndex(
                name: "IX_PaymentSchedules_EnrollmentStudentId_EnrollmentClassId",
                table: "PaymentSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "EnrollmentClassId",
                table: "PaymentSchedules");

            migrationBuilder.DropColumn(
                name: "EnrollmentStudentId",
                table: "PaymentSchedules");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Enrollments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedules_EnrollmentId",
                table: "PaymentSchedules",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId",
                table: "Enrollments",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentSchedules_Enrollments_EnrollmentId",
                table: "PaymentSchedules",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
