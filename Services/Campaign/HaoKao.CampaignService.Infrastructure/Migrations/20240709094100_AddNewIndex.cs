using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CampaignService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CampaignService.Infrastructure.Migrations
{
    public partial class AddNewIndex : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<GiftBagReceiveLog>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GiftBagReceiveLog_ReceiverId_GiftBagId",
                    table: GetShardingTableName<GiftBagReceiveLog>(),
                    columns: new[] { "ReceiverId", "GiftBagId" });
            }


            if (IsCreateShardingTable<GiftBag>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GiftBag_StartTime_EndTime_IsPublished",
                    table: GetShardingTableName<GiftBag>(),
                    columns: new[] { "StartTime", "EndTime", "IsPublished" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<GiftBagReceiveLog>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_GiftBagReceiveLog_ReceiverId_GiftBagId",
                    table: GetShardingTableName<GiftBagReceiveLog>());
            }


            if (IsCreateShardingTable<GiftBag>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_GiftBag_StartTime_EndTime_IsPublished",
                    table: GetShardingTableName<GiftBag>());
            }

        }
    }
}
