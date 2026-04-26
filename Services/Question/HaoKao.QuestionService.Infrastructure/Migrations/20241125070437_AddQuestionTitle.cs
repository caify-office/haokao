using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class AddQuestionTitle : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "QuestionTitle",
                    table: GetShardingTableName<Question>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: false,
                    defaultValue: "",
                    comment: "试题标题 (管理端使用)")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Question_QuestionTitle",
                    table: GetShardingTableName<Question>(),
                    column: "QuestionTitle");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Question_QuestionTitle",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "QuestionTitle",
                    table: GetShardingTableName<Question>());
            }

        }
    }
}
