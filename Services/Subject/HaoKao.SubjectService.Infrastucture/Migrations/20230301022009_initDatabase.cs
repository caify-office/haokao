using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.SubjectService.Infrastructure.Migrations
{
    public partial class initDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "科目名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsCommon = table.Column<int>(type: "int", nullable: false, comment: "是否是专业科目"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
