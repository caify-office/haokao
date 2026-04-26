using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionWrongPaperMoudle;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class AddQuestionCountToQuestionWrongPaper : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionWrongPaper>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "QuestionCount",
                    table: GetShardingTableName<QuestionWrongPaper>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0,
                    comment: "试题数量");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionWrongPaper>())
            {
                migrationBuilder.DropColumn(
                    name: "QuestionCount",
                    table: GetShardingTableName<QuestionWrongPaper>());
            }

        }
    }
}
