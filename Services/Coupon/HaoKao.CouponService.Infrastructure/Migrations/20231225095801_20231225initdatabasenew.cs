using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20231225initdatabasenew : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.DropColumn(
                    name: "IsLocked",
                    table: GetShardingTableName<Coupon>());
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.DropColumn(
                    name: "ReceiveCount",
                    table: GetShardingTableName<Coupon>());
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IsLocked",
                    table: GetShardingTableName<Coupon>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false);
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "ReceiveCount",
                    table: GetShardingTableName<Coupon>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }

        }
    }
}
