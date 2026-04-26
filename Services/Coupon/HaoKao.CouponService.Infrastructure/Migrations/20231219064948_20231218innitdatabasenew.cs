using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20231218innitdatabasenew : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "Hour",
                    table: GetShardingTableName<Coupon>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "Scope",
                    table: GetShardingTableName<Coupon>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "TimeType",
                    table: GetShardingTableName<Coupon>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.DropColumn(
                    name: "Hour",
                    table: GetShardingTableName<Coupon>());
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.DropColumn(
                    name: "Scope",
                    table: GetShardingTableName<Coupon>());
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.DropColumn(
                    name: "TimeType",
                    table: GetShardingTableName<Coupon>());
            }

        }
    }
}
