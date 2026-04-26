using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class addIosIsOpen : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<PlatformPayer>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IosIsOpen",
                    table: GetShardingTableName<PlatformPayer>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false,
                    comment: "Ios是否开启");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<PlatformPayer>())
            {
                migrationBuilder.DropColumn(
                    name: "IosIsOpen",
                    table: GetShardingTableName<PlatformPayer>());
            }

        }
    }
}
