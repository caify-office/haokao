using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LearnProgressService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearnProgressService.Infrastructure.Migrations
{
    public partial class innitdatabase20230901 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IsEnd",
                    table: GetShardingTableName<LearnProgress>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.DropColumn(
                    name: "IsEnd",
                    table: GetShardingTableName<LearnProgress>());
            }

        }
    }
}
