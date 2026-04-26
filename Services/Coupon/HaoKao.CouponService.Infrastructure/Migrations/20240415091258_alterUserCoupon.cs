using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class alterUserCoupon : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "ChannelType",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "Notified",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false,
                    comment: "是否已通知");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "OpenId",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "varchar(255)",
                    maxLength: 255,
                    nullable: true,
                    comment: "OpenId")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "ChannelType",
                    table: GetShardingTableName<UserCoupon>());
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "Notified",
                    table: GetShardingTableName<UserCoupon>());
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "OpenId",
                    table: GetShardingTableName<UserCoupon>());
            }

        }
    }
}
