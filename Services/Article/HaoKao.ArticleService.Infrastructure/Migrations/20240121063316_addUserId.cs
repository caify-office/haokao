using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ArticleService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ArticleService.Infrastructure.Migrations
{
    public partial class addUserId : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_ArticleBrowseRecord_ArticleId_TenantId",
                    table: GetShardingTableName<ArticleBrowseRecord>());
            }


            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "UserId",
                    table: GetShardingTableName<ArticleBrowseRecord>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_ArticleBrowseRecord_ArticleId_UserId_TenantId",
                    table: GetShardingTableName<ArticleBrowseRecord>());
            }


            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.DropColumn(
                    name: "UserId",
                    table: GetShardingTableName<ArticleBrowseRecord>());
            }


            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ArticleBrowseRecord_ArticleId_TenantId",
                    table: GetShardingTableName<ArticleBrowseRecord>(),
                    columns: new[] { "ArticleId", "TenantId" });
            }

        }
    }
}
