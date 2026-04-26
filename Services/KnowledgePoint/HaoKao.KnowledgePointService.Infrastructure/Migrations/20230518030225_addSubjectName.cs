using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.KnowledgePointService.Infrastructure.Migrations
{
    public partial class addSubjectName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ChpaterNodeId",
                table: "KnowledgePoint",
                type: "varchar(36)",
                nullable: false,
                comment: "章节Id",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldComment: "章节Id")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SubjectName",
                table: "KnowledgePoint",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectName",
                table: "KnowledgePoint");

            migrationBuilder.AlterColumn<string>(
                name: "ChpaterNodeId",
                table: "KnowledgePoint",
                type: "varchar(50)",
                nullable: false,
                comment: "章节Id",
                oldClrType: typeof(string),
                oldType: "varchar(36)",
                oldComment: "章节Id")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
