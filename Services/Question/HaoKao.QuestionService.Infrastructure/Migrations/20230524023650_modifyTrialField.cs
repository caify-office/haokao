using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class modifyTrialField : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "TrialQuestionId",
                    table: GetShardingTableName<Question>(),
                    type: "varchar(50)",
                    nullable: true,
                    comment: "命审题Id",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "TrialQuestionId",
                    table: GetShardingTableName<Question>(),
                    type: "char(36)",
                    nullable: true,
                    collation: "ascii_general_ci",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldNullable: true,
                    oldComment: "命审题Id")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}