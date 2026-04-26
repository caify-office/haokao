using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.KnowledgePointService.Infrastructure.Migrations
{
    public partial class initDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KnowledgePoint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false, comment: "知识点名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChpaterNodeId = table.Column<string>(type: "varchar(50)", nullable: false, comment: "章节Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChpaterNodeName = table.Column<string>(type: "varchar(100)", nullable: false, comment: "章节名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remark = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgePoint", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgePoint_ChpaterNodeId_Name_TenantId",
                table: "KnowledgePoint",
                columns: new[] { "ChpaterNodeId", "Name", "TenantId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KnowledgePoint");
        }
    }
}
