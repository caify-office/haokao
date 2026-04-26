using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.PaperService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.PaperService.Infrastructure.Migrations
{
    public partial class AddYear : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Paper>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "Year",
                    table: GetShardingTableName<Paper>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 2024,
                    comment: "年份");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Paper>())
            {
                migrationBuilder.DropColumn(
                    name: "Year",
                    table: GetShardingTableName<Paper>());
            }

        }
    }
}
