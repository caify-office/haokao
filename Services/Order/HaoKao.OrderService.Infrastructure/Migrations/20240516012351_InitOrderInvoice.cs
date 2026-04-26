using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class InitOrderInvoice : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<OrderInvoice>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<OrderInvoice>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        OrderId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "订单Id", collation: "ascii_general_ci"),
                        InvoiceType = table.Column<int>(type: "int", nullable: false, comment: "发票类型"),
                        VatInvoiceType = table.Column<int>(type: "int", nullable: false, comment: "增值税发票类型"),
                        InvoiceTitle = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "发票抬头")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TaxNo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "税号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        RegistryAddress = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true, comment: "注册地址")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        RegistryTel = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "注册电话")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        BankName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "开户银行")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        BankAccount = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "银行账号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        InvoiceFormat = table.Column<int>(type: "int", nullable: false, comment: "发票形式(获取方式)"),
                        Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "电子邮箱")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ShippingArea = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, comment: "所在地区")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ShippingAddress = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "收件人地址")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ReceiverName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "收件人姓名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ReceiverPhone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "收件人联系电话")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        RequestState = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "申请状态"),
                        ShippingNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "物流单号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        LogisticsCompany = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "物流公司")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ShippingTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "发货时间"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "创建者")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "创建者名称")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_OrderInvoice", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<OrderInvoice>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_OrderInvoice_OrderId",
                    table: GetShardingTableName<OrderInvoice>(),
                    column: "OrderId",
                    unique: true);
            }


            if (IsCreateShardingTable<OrderInvoice>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_OrderInvoice_OrderId_TenantId",
                    table: GetShardingTableName<OrderInvoice>(),
                    columns: new[] { "OrderId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<OrderInvoice>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<OrderInvoice>());
            }

        }
    }
}
