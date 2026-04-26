using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.WebsiteConfigurationService.Infrastructure.Migrations
{
    public partial class addColumnSort : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "Sort",
                    table: GetShardingTableName<Column>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.DropColumn(
                    name: "Sort",
                    table: GetShardingTableName<Column>());
            }

        }
    }
}
