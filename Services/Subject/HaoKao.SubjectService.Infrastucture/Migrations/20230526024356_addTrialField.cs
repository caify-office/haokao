using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.SubjectService.Infrastructure.Migrations
{
    public partial class addTrialField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subject",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "科目名称",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "科目名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TrialSubjectId",
                table: "Subject",
                type: "varchar(50)",
                nullable: true,
                comment: "命审题科目Id")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_Name_TenantId",
                table: "Subject",
                columns: new[] { "Name", "TenantId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subject_Name_TenantId",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "TrialSubjectId",
                table: "Subject");

            migrationBuilder.UpdateData(
                table: "Subject",
                keyColumn: "Name",
                keyValue: null,
                column: "Name",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subject",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "科目名称",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "科目名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
