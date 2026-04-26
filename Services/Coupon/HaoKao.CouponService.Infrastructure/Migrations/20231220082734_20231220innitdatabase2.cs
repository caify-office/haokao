using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20231220innitdatabase2 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<decimal>(
                    name: "FactAmount",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "decimal(65,30)",
                    nullable: false,
                    defaultValue: 0m);
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IsLock",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false);
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "NickName",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "longtext",
                    nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "ProductName",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "longtext",
                    nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "TelPhone",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "longtext",
                    nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "FactAmount",
                    table: GetShardingTableName<UserCoupon>());
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "IsLock",
                    table: GetShardingTableName<UserCoupon>());
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "NickName",
                    table: GetShardingTableName<UserCoupon>());
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "ProductName",
                    table: GetShardingTableName<UserCoupon>());
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "TelPhone",
                    table: GetShardingTableName<UserCoupon>());
            }


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
    }
}
