using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.PaperService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.PaperService.Infrastructure.Migrations
{
    public partial class addAndQustionCountTotalScore : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Paper>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "QuestionCount",
                    table: GetShardingTableName<Paper>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0,
                    comment: "总题数");
            }


            if (IsCreateShardingTable<Paper>())
            {
                migrationBuilder.AddColumn<decimal>(
                    name: "TotalScore",
                    table: GetShardingTableName<Paper>(),
                    type: "decimal(6,2)",
                    nullable: false,
                    defaultValue: 0m,
                    comment: "总分数");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Paper>())
            {
                migrationBuilder.DropColumn(
                    name: "QuestionCount",
                    table: GetShardingTableName<Paper>());
            }


            if (IsCreateShardingTable<Paper>())
            {
                migrationBuilder.DropColumn(
                    name: "TotalScore",
                    table: GetShardingTableName<Paper>());
            }

        }
    }
}
