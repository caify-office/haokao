using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20231220innitdatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "OrderId",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "OrderNo",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "longtext",
                    nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "OrderId",
                    table: GetShardingTableName<UserCoupon>());
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "OrderNo",
                    table: GetShardingTableName<UserCoupon>());
            }

        }
    }
}
