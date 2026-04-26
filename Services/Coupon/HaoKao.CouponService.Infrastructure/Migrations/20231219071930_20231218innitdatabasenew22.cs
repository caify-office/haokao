using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20231218innitdatabasenew22 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "ThresholdAmount",
                    table: GetShardingTableName<Coupon>(),
                    type: "decimal(18,2)",
                    maxLength: 20,
                    precision: 18,
                    scale: 2,
                    nullable: false,
                    comment: "门槛金额",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(65,30)");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "ThresholdAmount",
                    table: GetShardingTableName<Coupon>(),
                    type: "decimal(65,30)",
                    nullable: false,
                    oldClrType: typeof(decimal),
                    oldType: "decimal(18,2)",
                    oldMaxLength: 20,
                    oldPrecision: 18,
                    oldScale: 2,
                    oldComment: "门槛金额");
            }

        }
    }
}
