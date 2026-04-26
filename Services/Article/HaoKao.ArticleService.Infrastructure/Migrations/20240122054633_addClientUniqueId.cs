using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ArticleService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ArticleService.Infrastructure.Migrations
{
    public partial class addClientUniqueId : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_ArticleBrowseRecord_ArticleId_UserId_TenantId",
                    table: GetShardingTableName<ArticleBrowseRecord>());
            }


            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.RenameColumn(
                    name: "UserId",
                    table: GetShardingTableName<ArticleBrowseRecord>(),
                    newName: "ClientUniqueId");
            }


            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ArticleBrowseRecord_ArticleId_ClientUniqueId_TenantId",
                    table: GetShardingTableName<ArticleBrowseRecord>(),
                    columns: new[] { "ArticleId", "ClientUniqueId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_ArticleBrowseRecord_ArticleId_ClientUniqueId_TenantId",
                    table: GetShardingTableName<ArticleBrowseRecord>());
            }


            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.RenameColumn(
                    name: "ClientUniqueId",
                    table: GetShardingTableName<ArticleBrowseRecord>(),
                    newName: "UserId");
            }


            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ArticleBrowseRecord_ArticleId_UserId_TenantId",
                    table: GetShardingTableName<ArticleBrowseRecord>(),
                    columns: new[] { "ArticleId", "UserId", "TenantId" },
                    unique: true);
            }

        }
    }
}
