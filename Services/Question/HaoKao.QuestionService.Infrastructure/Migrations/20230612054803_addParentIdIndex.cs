using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class addParentIdIndex : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Question_SubjectId_ChapterNodeId_TenantId",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Question_ParentId",
                    table: GetShardingTableName<Question>(),
                    column: "ParentId");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Question_SubjectId_ChapterNodeId_QuestionCategoryId_TenantId",
                    table: GetShardingTableName<Question>(),
                    columns: new[] { "SubjectId", "ChapterNodeId", "QuestionCategoryId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Question_ParentId",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Question_SubjectId_ChapterNodeId_QuestionCategoryId_TenantId",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Question_SubjectId_ChapterNodeId_TenantId",
                    table: GetShardingTableName<Question>(),
                    columns: new[] { "SubjectId", "ChapterNodeId", "TenantId" });
            }

        }
    }
}