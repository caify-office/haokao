using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20140119innitdatabase1 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "ProductPackageId",
                    table: GetShardingTableName<Coupon>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.DropColumn(
                    name: "ProductPackageId",
                    table: GetShardingTableName<Coupon>());
            }

        }
    }
}
