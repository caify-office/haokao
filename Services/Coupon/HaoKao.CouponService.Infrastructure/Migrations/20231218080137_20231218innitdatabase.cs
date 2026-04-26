using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20231218innitdatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Coupon>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        EndDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "有效期-开始时间"),
                        BeginDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "有效期-结束时间"),
                        CouponCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "优惠券卡号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CouponName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "优惠券名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CouponDesc = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "优惠券说明")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PersonName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "助教名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CouponType = table.Column<int>(type: "int", nullable: false),
                        ProductIds = table.Column<string>(type: "text", nullable: true, comment: "适用产品:通过产品包-课程逐级筛选，支持多选和反选。不选产品时，该租户下全场产品通用选择产品集合")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "生成的url链接地址")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Amount = table.Column<decimal>(type: "decimal(18,2)", maxLength: 20, precision: 18, scale: 2, nullable: false, comment: "金额"),
                        IsOnlyName = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Coupon", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<UserCoupon>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CouponId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        IsUse = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_UserCoupon", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<UserCoupon>("FK_UserCoupon_Coupon_CouponId"),
                            column: x => x.CouponId,
                            principalTable: GetShardingTableName<Coupon>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UserCoupon_CouponId",
                    table: GetShardingTableName<UserCoupon>(),
                    column: "CouponId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCoupon>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<UserCoupon>());
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Coupon>());
            }

        }
    }
}
