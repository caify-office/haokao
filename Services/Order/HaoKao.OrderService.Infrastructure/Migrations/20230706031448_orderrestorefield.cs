using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class orderrestorefield : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IosRestorePurchase",
                    table: GetShardingTableName<Order>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.DropColumn(
                    name: "IosRestorePurchase",
                    table: GetShardingTableName<Order>());
            }

        }
    }
}
