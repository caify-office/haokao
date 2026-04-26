using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class InitProductSales : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ProductSales>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ProductSales>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "产品Id", collation: "ascii_general_ci"),
                        ProductName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "产品名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ActualPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "价格"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ProductSales", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ProductSales>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ProductSales_ProductId_CreateTime_TenantId",
                    table: GetShardingTableName<ProductSales>(),
                    columns: new[] { "ProductId", "CreateTime", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ProductSales>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ProductSales>());
            }

        }
    }
}
