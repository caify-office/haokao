using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.WebsiteConfigurationService.Infrastructure.Migrations
{
    public partial class addActiveIcon : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "ActiveIcon",
                    table: GetShardingTableName<Column>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "当前图标")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.DropColumn(
                    name: "ActiveIcon",
                    table: GetShardingTableName<Column>());
            }

        }
    }
}
