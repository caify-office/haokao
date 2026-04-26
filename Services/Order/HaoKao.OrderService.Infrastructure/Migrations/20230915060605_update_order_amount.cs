using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class update_order_amount : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "PurchaseProductContents",
                    table: GetShardingTableName<Order>(),
                    type: "json",
                    nullable: true,
                    comment: "购买商品详细内容",
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldNullable: true,
                    oldComment: "购买商品详细内容")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "OrderAmount",
                    table: GetShardingTableName<Order>(),
                    type: "decimal(10,2)",
                    precision: 10,
                    scale: 2,
                    nullable: false,
                    comment: "订单金额",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(6,2)",
                    oldPrecision: 6,
                    oldScale: 2,
                    oldComment: "订单金额");
            }


            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "ActualAmount",
                    table: GetShardingTableName<Order>(),
                    type: "decimal(10,2)",
                    precision: 10,
                    scale: 2,
                    nullable: false,
                    comment: "实际金额",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(6,2)",
                    oldPrecision: 6,
                    oldScale: 2,
                    oldComment: "实际金额");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "PurchaseProductContents",
                    table: GetShardingTableName<Order>(),
                    type: "text",
                    nullable: true,
                    comment: "购买商品详细内容",
                    oldClrType: typeof(string),
                    oldType: "json",
                    oldNullable: true,
                    oldComment: "购买商品详细内容")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "OrderAmount",
                    table: GetShardingTableName<Order>(),
                    type: "decimal(6,2)",
                    precision: 6,
                    scale: 2,
                    nullable: false,
                    comment: "订单金额",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(10,2)",
                    oldPrecision: 10,
                    oldScale: 2,
                    oldComment: "订单金额");
            }


            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "ActualAmount",
                    table: GetShardingTableName<Order>(),
                    type: "decimal(6,2)",
                    precision: 6,
                    scale: 2,
                    nullable: false,
                    comment: "实际金额",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(10,2)",
                    oldPrecision: 10,
                    oldScale: 2,
                    oldComment: "实际金额");
            }

        }
    }
}