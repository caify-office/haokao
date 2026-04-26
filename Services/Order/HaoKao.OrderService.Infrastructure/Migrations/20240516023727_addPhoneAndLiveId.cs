using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class addPhoneAndLiveId : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "LiveId",
                    table: GetShardingTableName<Order>(),
                    type: "char(36)",
                    nullable: true,
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "Phone",
                    table: GetShardingTableName<Order>(),
                    type: "varchar(11)",
                    nullable: true,
                    comment: "手机号码")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.DropColumn(
                    name: "LiveId",
                    table: GetShardingTableName<Order>());
            }


            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.DropColumn(
                    name: "Phone",
                    table: GetShardingTableName<Order>());
            }


        }
    }
}
