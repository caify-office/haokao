using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20231218innitdatabasenew1 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AddColumn<decimal>(
                    name: "ThresholdAmount",
                    table: GetShardingTableName<Coupon>(),
                    type: "decimal(65,30)",
                    nullable: false,
                    defaultValue: 0m);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.DropColumn(
                    name: "ThresholdAmount",
                    table: GetShardingTableName<Coupon>());
            }

        }
    }
}
