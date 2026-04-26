using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20240104innitdatabase1 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCouponPerformance>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<UserCouponPerformance>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        OrderNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "订单编号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        OrderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        TelPhone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号码--冗余")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        NickName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "昵称--冗余")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ProductName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "产品名称--冗余")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        FactAmount = table.Column<decimal>(type: "decimal(18,2)", maxLength: 20, precision: 18, scale: 2, nullable: false, comment: "实际支付金额--冗余"),
                        Amount = table.Column<decimal>(type: "decimal(18,2)", maxLength: 20, precision: 18, scale: 2, nullable: false, comment: "产品原价--冗余"),
                        PayTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "支付时间--冗余"),
                        Remark = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "备注--后台手动添加的默认手动添加")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PersonName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "助教实名名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PersonUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_UserCouponPerformance", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserCouponPerformance>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<UserCouponPerformance>());
            }

        }
    }
}
