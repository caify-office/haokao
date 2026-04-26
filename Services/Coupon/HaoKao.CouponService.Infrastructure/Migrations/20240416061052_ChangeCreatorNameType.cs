using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class ChangeCreatorNameType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCouponPerformance>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<UserCouponPerformance>(),
                    type: "varchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(40)",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "varchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(40)",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "ProductIds",
                    table: GetShardingTableName<Coupon>(),
                    type: "json",
                    nullable: true,
                    comment: "适用产品:通过产品包-课程逐级筛选，支持多选和反选。不选产品时，该租户下全场产品通用选择产品集合",
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldNullable: true,
                    oldComment: "适用产品:通过产品包-课程逐级筛选，支持多选和反选。不选产品时，该租户下全场产品通用选择产品集合")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCouponPerformance>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<UserCouponPerformance>(),
                    type: "nvarchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "varchar(40)",
                    oldNullable: true)
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "nvarchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "varchar(40)",
                    oldNullable: true)
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "ProductIds",
                    table: GetShardingTableName<Coupon>(),
                    type: "text",
                    nullable: true,
                    comment: "适用产品:通过产品包-课程逐级筛选，支持多选和反选。不选产品时，该租户下全场产品通用选择产品集合",
                    oldClrType: typeof(string),
                    oldType: "json",
                    oldNullable: true,
                    oldComment: "适用产品:通过产品包-课程逐级筛选，支持多选和反选。不选产品时，该租户下全场产品通用选择产品集合")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
