using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.DailyQuestionModule;
using HaoKao.QuestionService.Domain.QuestionWrongModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class addCreateTimeToQuestionWrong : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DailyQuestion>())
            {
                migrationBuilder.AlterTable(
                    name: GetShardingTableName<DailyQuestion>(),
                    comment: "每日一题",
                    oldComment: "每日一提")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.AddColumn<DateTime>(
                    name: "CreateTime",
                    table: GetShardingTableName<QuestionWrong>(),
                    type: "datetime",
                    nullable: false,
                    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.DropColumn(
                    name: "CreateTime",
                    table: GetShardingTableName<QuestionWrong>());
            }


            if (IsCreateShardingTable<DailyQuestion>())
            {
                migrationBuilder.AlterTable(
                    name: GetShardingTableName<DailyQuestion>(),
                    comment: "每日一提",
                    oldComment: "每日一题")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}