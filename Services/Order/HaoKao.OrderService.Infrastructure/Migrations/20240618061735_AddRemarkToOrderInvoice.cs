using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class AddRemarkToOrderInvoice : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<OrderInvoice>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "Remark",
                    table: GetShardingTableName<OrderInvoice>(),
                    type: "varchar(200)",
                    maxLength: 200,
                    nullable: true,
                    comment: "发票备注")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<OrderInvoice>())
            {
                migrationBuilder.DropColumn(
                    name: "Remark",
                    table: GetShardingTableName<OrderInvoice>());
            }

        }
    }
}
