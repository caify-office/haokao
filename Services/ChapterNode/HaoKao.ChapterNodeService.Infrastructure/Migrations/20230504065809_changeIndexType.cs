using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ChapterNodeService.Infrastructure.Migrations
{
    public partial class changeIndexType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_ChapterNode_ParentId",
                    table: GetShardingTableName<ChapterNode>());
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_ChapterNode_SubjectId",
                    table: GetShardingTableName<ChapterNode>());
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_ChapterNode_TenantId",
                    table: GetShardingTableName<ChapterNode>());
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ChapterNode_SubjectId_ParentId_TenantId",
                    table: GetShardingTableName<ChapterNode>(),
                    columns: new[] { "SubjectId", "ParentId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_ChapterNode_SubjectId_ParentId_TenantId",
                    table: GetShardingTableName<ChapterNode>());
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
    }
}
