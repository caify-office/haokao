using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionCollectionModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class addCreateTimeToQuestionCollection : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCollection>())
            {
                migrationBuilder.AddColumn<DateTime>(
                    name: "CreateTime",
                    table: GetShardingTableName<QuestionCollection>(),
                    type: "datetime",
                    nullable: false,
                    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCollection>())
            {
                migrationBuilder.DropColumn(
                    name: "CreateTime",
                    table: GetShardingTableName<QuestionCollection>());
            }

        }
    }
}