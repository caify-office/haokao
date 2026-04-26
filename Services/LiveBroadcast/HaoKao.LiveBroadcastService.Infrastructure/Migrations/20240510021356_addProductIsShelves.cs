using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LiveBroadcastService.Infrastructure.Migrations
{
    public partial class addProductIsShelves : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LiveProductPackage>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IsShelves",
                    table: GetShardingTableName<LiveProductPackage>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LiveProductPackage>())
            {
                migrationBuilder.DropColumn(
                    name: "IsShelves",
                    table: GetShardingTableName<LiveProductPackage>());
            }

        }
    }
}
