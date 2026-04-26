using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20240115innitdatabasenew : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "Amount",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "decimal(18,2)",
                    maxLength: 20,
                    precision: 18,
                    scale: 2,
                    nullable: false,
                    comment: "订单金额",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(65,30)");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "Amount",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "decimal(65,30)",
                    nullable: false,
                    oldClrType: typeof(decimal),
                    oldType: "decimal(18,2)",
                    oldMaxLength: 20,
                    oldPrecision: 18,
                    oldScale: 2,
                    oldComment: "订单金额");
            }

        }
    }
}
