using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Migrations
{
    public partial class rmAnswerRecordId : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserAnswerRecord>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_UserAnswerRecord_2023_SubjectId_QuestionCategoryId_RecordIde~",
                    table: GetShardingTableName<UserAnswerRecord>());
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_UserAnswerQuestion_2023_AnswerRecordId_QuestionId_TenantId",
                    table: GetShardingTableName<UserAnswerQuestion>());
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.DropColumn(
                    name: "AnswerRecordId",
                    table: GetShardingTableName<UserAnswerQuestion>());
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "UserAnswerRecordId",
                    table: GetShardingTableName<UserAnswerQuestion>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)",
                    oldNullable: true)
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }


            if (IsCreateShardingTable<UserAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UserAnswerRecord_2023_SubjectId_QuestionCategoryId_RecordIde~",
                    table: GetShardingTableName<UserAnswerRecord>(),
                    columns: new[] { "SubjectId", "QuestionCategoryId", "RecordIdentifier", "CreatorId", "CreateTime", "TenantId" });
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UserAnswerQuestion_2023_QuestionId_TenantId",
                    table: GetShardingTableName<UserAnswerQuestion>(),
                    columns: new[] { "QuestionId", "TenantId" });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserAnswerRecord>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_UserAnswerRecord_2023_SubjectId_QuestionCategoryId_RecordIde~",
                    table: GetShardingTableName<UserAnswerRecord>());
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_UserAnswerQuestion_2023_QuestionId_TenantId",
                    table: GetShardingTableName<UserAnswerQuestion>());
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "UserAnswerRecordId",
                    table: GetShardingTableName<UserAnswerQuestion>(),
                    type: "char(36)",
                    nullable: true,
                    collation: "ascii_general_ci",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)")
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "AnswerRecordId",
                    table: GetShardingTableName<UserAnswerQuestion>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    comment: "答题记录Id",
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<UserAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UserAnswerRecord_2023_SubjectId_QuestionCategoryId_RecordIde~",
                    table: GetShardingTableName<UserAnswerRecord>(),
                    columns: new[] { "SubjectId", "QuestionCategoryId", "RecordIdentifier", "CreatorId", "TenantId" });
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UserAnswerQuestion_2023_AnswerRecordId_QuestionId_TenantId",
                    table: GetShardingTableName<UserAnswerQuestion>(),
                    columns: new[] { "AnswerRecordId", "QuestionId", "TenantId" });
            }
        }
    }
}