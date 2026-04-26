using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class AddPaperTagIdForQuestion : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "PaperTagId",
                    table: GetShardingTableName<Question>(),
                    type: "char(36)",
                    nullable: true,
                    comment: "试卷标签Id",
                    collation: "ascii_general_ci");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "PaperTagId",
                    table: GetShardingTableName<Question>());
            }

        }
    }
}
