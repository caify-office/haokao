using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionModule;
using HaoKao.QuestionService.Domain.QuestionWrongModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class ModifyQuestionWrong : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "ParentQuestionId",
                    table: GetShardingTableName<QuestionWrong>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    comment: "父试题Id",
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "ParentQuestionTypeId",
                    table: GetShardingTableName<QuestionWrong>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    comment: "父试题类型Id",
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "QuestionTypeId",
                    table: GetShardingTableName<QuestionWrong>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    comment: "试题类型Id",
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_QuestionWrong_QuestionId_CreatorId_QuestionTypeId_ParentQues~",
                    table: GetShardingTableName<QuestionWrong>(),
                    columns: new[] { "QuestionId", "CreatorId", "QuestionTypeId", "ParentQuestionTypeId", "TenantId" });
            }


            if (IsCreateShardingTable<QuestionWrong>())
            {
                var question = GetShardingTableName<Question>();
                var questionWrong = GetShardingTableName<QuestionWrong>();
                migrationBuilder.Sql(@$"UPDATE {questionWrong} qw
JOIN {question} q ON qw.QuestionId = q.Id
JOIN {question} parent ON q.ParentId = parent.Id
SET qw.QuestionTypeId = q.QuestionTypeId,
    qw.ParentQuestionId = q.ParentId,
    qw.ParentQuestionTypeId = parent.QuestionTypeId;");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_QuestionWrong_QuestionId_CreatorId_QuestionTypeId_ParentQues~",
                    table: GetShardingTableName<QuestionWrong>());
            }


            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.DropColumn(
                    name: "ParentQuestionId",
                    table: GetShardingTableName<QuestionWrong>());
            }


            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.DropColumn(
                    name: "ParentQuestionTypeId",
                    table: GetShardingTableName<QuestionWrong>());
            }


            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.DropColumn(
                    name: "QuestionTypeId",
                    table: GetShardingTableName<QuestionWrong>());
            }

        }
    }
}
