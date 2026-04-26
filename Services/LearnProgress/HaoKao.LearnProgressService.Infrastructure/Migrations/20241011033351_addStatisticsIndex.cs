using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LearnProgressService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearnProgressService.Infrastructure.Migrations
{
    public partial class addStatisticsIndex : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LearnProgress_CreatorId_VideoId_TenantId",
                    table: GetShardingTableName<LearnProgress>(),
                    columns: new[] { "CreatorId", "VideoId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_LearnProgress_CreatorId_VideoId_TenantId",
                    table: GetShardingTableName<LearnProgress>());
            }

        }
    }
}
