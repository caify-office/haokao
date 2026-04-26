using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ArticleService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ArticleService.Infrastructure.Migrations
{
    public partial class addArticleIdIndex : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ArticleBrowseRecord_ArticleId_TenantId",
                    table: GetShardingTableName<ArticleBrowseRecord>(),
                    columns: new[] { "ArticleId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_ArticleBrowseRecord_ArticleId_TenantId",
                    table: GetShardingTableName<ArticleBrowseRecord>());
            }

        }
    }
}
