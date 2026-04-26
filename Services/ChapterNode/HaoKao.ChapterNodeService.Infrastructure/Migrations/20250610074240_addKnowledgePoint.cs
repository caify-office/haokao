using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ChapterNodeService.Infrastructure.Migrations
{
    public partial class addKnowledgePoint : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<KnowledgePoint>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<KnowledgePoint>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(100)", nullable: false, comment: "知识点名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SubjectName = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ChapterNodeId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "章节Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ChapterNodeName = table.Column<string>(type: "varchar(100)", nullable: false, comment: "章节名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ExamFrequency = table.Column<int>(type: "int", nullable: false),
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
            }


            if (IsCreateShardingTable<KnowledgePoint>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_KnowledgePoint_ChapterNodeId_Name_TenantId",
                    table: GetShardingTableName<KnowledgePoint>(),
                    columns: new[] { "ChapterNodeId", "Name", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<KnowledgePoint>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<KnowledgePoint>());
            }

        }
    }
}
