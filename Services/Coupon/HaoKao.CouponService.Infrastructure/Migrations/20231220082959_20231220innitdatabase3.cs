using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20231220innitdatabase3 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "TelPhone",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "手机号码",
                    oldClrType: typeof(string),
                    oldType: "longtext",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "ProductName",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "产品名称",
                    oldClrType: typeof(string),
                    oldType: "longtext",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "NickName",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "varchar(200)",
                    maxLength: 200,
                    nullable: true,
                    comment: "昵称",
                    oldClrType: typeof(string),
                    oldType: "longtext",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "FactAmount",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "decimal(18,2)",
                    maxLength: 20,
                    precision: 18,
                    scale: 2,
                    nullable: false,
                    comment: "实际支付金额",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(65,30)");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "TelPhone",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "longtext",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "手机号码")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "ProductName",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "longtext",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "varchar(500)",
                    oldMaxLength: 500,
                    oldNullable: true,
                    oldComment: "产品名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "NickName",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "longtext",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "varchar(200)",
                    oldMaxLength: 200,
                    oldNullable: true,
                    oldComment: "昵称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "FactAmount",
                    table: GetShardingTableName<UserCoupon>(),
                    type: "decimal(65,30)",
                    nullable: false,
                    oldClrType: typeof(decimal),
                    oldType: "decimal(18,2)",
                    oldMaxLength: 20,
                    oldPrecision: 18,
                    oldScale: 2,
                    oldComment: "实际支付金额");
            }

        }
    }
}
