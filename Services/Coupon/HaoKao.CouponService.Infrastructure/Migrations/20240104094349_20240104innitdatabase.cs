using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20240104innitdatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<decimal>(
                    name: "Amount",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "decimal(65,30)",
                    nullable: false,
                    defaultValue: 0m);
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<DateTime>(
                    name: "PayTime",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "datetime(6)",
                    nullable: false,
                    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "Remark",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "longtext",
                    nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "PersonUserId",
                    table: GetShardingTableName<Coupon>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "Amount",
                    table: GetShardingTableName<UserCoupon>());
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "PayTime",
                    table: GetShardingTableName<UserCoupon>());
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "Remark",
                    table: GetShardingTableName<UserCoupon>());
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.DropColumn(
                    name: "PersonUserId",
                    table: GetShardingTableName<Coupon>());
            }

        }
    }
}
