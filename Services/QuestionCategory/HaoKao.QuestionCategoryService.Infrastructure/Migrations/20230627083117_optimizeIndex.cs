using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionCategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionCategoryService.Infrastructure.Migrations
{
    public partial class optimizeIndex : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_QuestionCategory_Name_TenantId",
                    table: GetShardingTableName<QuestionCategory>());
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_QuestionCategory_AdaptPlace_Name_TenantId",
                    table: GetShardingTableName<QuestionCategory>(),
                    columns: new[] { "AdaptPlace", "Name", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_QuestionCategory_AdaptPlace_Name_TenantId",
                    table: GetShardingTableName<QuestionCategory>());
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_QuestionCategory_Name_TenantId",
                    table: GetShardingTableName<QuestionCategory>(),
                    columns: new[] { "Name", "TenantId" });
            }

        }
    }
}
