using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionWrongModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class AddQuestionWrongSort : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "Sort",
                    table: GetShardingTableName<QuestionWrong>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0,
                    comment: "排序");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.DropColumn(
                    name: "Sort",
                    table: GetShardingTableName<QuestionWrong>());
            }

        }
    }
}
