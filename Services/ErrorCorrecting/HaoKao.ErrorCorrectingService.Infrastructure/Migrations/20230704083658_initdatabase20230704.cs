using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ErrorCorrectingService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ErrorCorrectingService.Infrastructure.Migrations
{
    public partial class initdatabase20230704 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ErrorCorrecting>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "Status",
                    table: GetShardingTableName<ErrorCorrecting>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ErrorCorrecting>())
            {
                migrationBuilder.DropColumn(
                    name: "Status",
                    table: GetShardingTableName<ErrorCorrecting>());
            }

        }
    }
}
