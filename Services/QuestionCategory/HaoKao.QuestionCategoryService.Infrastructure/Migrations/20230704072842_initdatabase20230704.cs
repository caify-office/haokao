using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionCategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionCategoryService.Infrastructure.Migrations
{
    public partial class initdatabase20230704 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IsShow",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.DropColumn(
                    name: "IsShow",
                    table: GetShardingTableName<QuestionCategory>());
            }

        }
    }
}
