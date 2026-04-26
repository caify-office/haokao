using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class initDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Order>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        PlatformPayerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        PlatformPayerName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "使用的平台配置的支付者的名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        OrderSerialNumber = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, comment: "订单第三方流水号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        OrderNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "订单号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PurchaseProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        PurchaseName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "购买产品名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PurchaseProductType = table.Column<int>(type: "int", nullable: false),
                        OrderAmount = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false, comment: "订单金额"),
                        ActualAmount = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false, comment: "实际金额"),
                        OrderState = table.Column<int>(type: "int", nullable: false),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantName = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ClientName = table.Column<string>(type: "varchar(50)", nullable: true, comment: "购买商品详细内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PurchaseProductContents = table.Column<string>(type: "text", nullable: true, comment: "购买商品详细内容")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Order", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<PlatformPayer>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<PlatformPayer>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "支付名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PayerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        PayerName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "对应支付处理者名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PlatformPayerScenes = table.Column<int>(type: "int", nullable: false),
                        PaymentMethod = table.Column<int>(type: "int", nullable: false),
                        Config = table.Column<string>(type: "text", nullable: true, comment: "支付相关配置")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true),
                        SecurityCode = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, comment: "数据安全码")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间"),
                        UseState = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true, comment: "启用/禁用")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_PlatformPayer", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Order_OrderNumber",
                    table: GetShardingTableName<Order>(),
                    column: "OrderNumber",
                    unique: true);
            }


            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Order_OrderNumber_PurchaseName_CreatorName_CreateTime_Update~",
                    table: GetShardingTableName<Order>(),
                    columns: new[] { "OrderNumber", "PurchaseName", "CreatorName", "CreateTime", "UpdateTime", "OrderState", "PurchaseProductType", "TenantId" });
            }


            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Order_OrderSerialNumber",
                    table: GetShardingTableName<Order>(),
                    column: "OrderSerialNumber",
                    unique: true);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Order>());
            }


            if (IsCreateShardingTable<PlatformPayer>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<PlatformPayer>());
            }

        }
    }
}
