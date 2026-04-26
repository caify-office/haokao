using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class AddIndexForUnionQuestion : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UnionQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UnionQuestion_SubjectId_QuestionCategoryId_TenantId",
                    table: GetShardingTableName<UnionQuestion>(),
                    columns: new[] { "SubjectId", "QuestionCategoryId", "TenantId" });
            }


            if (IsCreateShardingTable<UnionQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UnionQuestion_TenantId",
                    table: GetShardingTableName<UnionQuestion>(),
                    column: "TenantId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UnionQuestion>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_UnionQuestion_SubjectId_QuestionCategoryId_TenantId",
                    table: GetShardingTableName<UnionQuestion>());
            }


            if (IsCreateShardingTable<UnionQuestion>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_UnionQuestion_TenantId",
                    table: GetShardingTableName<UnionQuestion>());
            }

        }
    }
}
