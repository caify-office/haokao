using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class addForeignKey : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var tenantId = EngineContext.Current.ClaimManager?.IdentityClaim?.TenantId;
            if (IsCreateShardingTable<ProductSales>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "ActualPrice",
                    table: GetShardingTableName<ProductSales>(),
                    type: "decimal(10,2)",
                    precision: 10,
                    scale: 2,
                    nullable: false,
                    comment: "价格",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(65,30)",
                    oldComment: "价格");
            }

            


            if (IsCreateShardingTable<OrderInvoice>())
            {
                migrationBuilder.AddForeignKey(
                    name: CreateFkName("FK_OrderInvoice_Order_OrderId", tenantId),
                    table: GetShardingTableName<OrderInvoice>(),
                    column: "OrderId",
                    principalTable: GetShardingTableName<Order>(),
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            }

        }

        private static string CreateFkName(string baseName, string tenantId)
        {
            return "FK_" + $"{baseName}_{tenantId}".ToMd5();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var tenantId = EngineContext.Current.ClaimManager?.IdentityClaim?.TenantId;
            if (IsCreateShardingTable<OrderInvoice>())
            {
                migrationBuilder.DropForeignKey(
                    name: CreateFkName("FK_OrderInvoice_Order_OrderId", tenantId),
                    table: GetShardingTableName<OrderInvoice>());
            }


            if (IsCreateShardingTable<OrderInvoice>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_OrderInvoice_OrderId",
                    table: GetShardingTableName<OrderInvoice>());
            }


            if (IsCreateShardingTable<ProductSales>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "ActualPrice",
                    table: GetShardingTableName<ProductSales>(),
                    type: "decimal(65,30)",
                    nullable: false,
                    comment: "价格",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(10,2)",
                    oldPrecision: 10,
                    oldScale: 2,
                    oldComment: "价格");
            }

        }
    }
}
