using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ChapterNodeService.Infrastructure.Migrations
{
    public partial class AddShardingTable : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ChapterNode>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "编码")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "章节名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                        ParentName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "父级名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Sort = table.Column<int>(type: "int", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ChapterNode", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ChapterNode_ParentId",
                    table: GetShardingTableName<ChapterNode>(),
                    column: "ParentId");
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ChapterNode_SubjectId",
                    table: GetShardingTableName<ChapterNode>(),
                    column: "SubjectId");
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ChapterNode_TenantId",
                    table: GetShardingTableName<ChapterNode>(),
                    column: "TenantId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ChapterNode>());
            }

        }
    }
}
