using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class addBeginTime : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AddColumn<DateTime>(
                    name: "BeginTime",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "datetime(6)",
                    nullable: false,
                    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropColumn(
                    name: "BeginTime",
                    table: GetShardingTableName<UserCoupon>());
            }

        }
    }
}
