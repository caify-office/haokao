using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class UpdateQuestion : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Question_SubjectId_ChapterNodeId_QuestionCategoryId_TenantId",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "CorrectCount",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "WrongCount",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "AbilityIds",
                    table: GetShardingTableName<Question>(),
                    type: "varchar(360)",
                    maxLength: 360,
                    nullable: true,
                    comment: "能力维度Id",
                    oldClrType: typeof(string),
                    oldType: "varchar(360)",
                    oldMaxLength: 360,
                    oldNullable: true,
                    oldComment: "能力维度id")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "ChapterId",
                    table: GetShardingTableName<Question>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    comment: "章节Id",
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "ChapterName",
                    table: GetShardingTableName<Question>(),
                    type: "varchar(100)",
                    maxLength: 100,
                    nullable: true,
                    comment: "章节名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "KnowledgePointId",
                    table: GetShardingTableName<Question>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    comment: "知识点Id",
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<Question>(),
                    type: "varchar(100)",
                    maxLength: 100,
                    nullable: true,
                    comment: "知识点名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "QuestionCount",
                    table: GetShardingTableName<Question>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 1,
                    comment: "试题数量");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "SectionId",
                    table: GetShardingTableName<Question>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    comment: "小节Id",
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "SectionName",
                    table: GetShardingTableName<Question>(),
                    type: "varchar(100)",
                    maxLength: 100,
                    nullable: true,
                    comment: "小节名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<DateTime>(
                    name: "UpdateTime",
                    table: GetShardingTableName<Question>(),
                    type: "datetime",
                    nullable: false,
                    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Question_SubjectId_ChapterId_SectionId_KnowledgePointId_Ques~",
                    table: GetShardingTableName<Question>(),
                    columns: new[] { "SubjectId", "ChapterId", "SectionId", "KnowledgePointId", "QuestionCategoryId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Question_SubjectId_ChapterId_SectionId_KnowledgePointId_Ques~",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "ChapterId",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "ChapterName",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "KnowledgePointId",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "QuestionCount",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "SectionId",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "SectionName",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropColumn(
                    name: "UpdateTime",
                    table: GetShardingTableName<Question>());
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "AbilityIds",
                    table: GetShardingTableName<Question>(),
                    type: "varchar(360)",
                    maxLength: 360,
                    nullable: true,
                    comment: "能力维度id",
                    oldClrType: typeof(string),
                    oldType: "varchar(360)",
                    oldMaxLength: 360,
                    oldNullable: true,
                    oldComment: "能力维度Id")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<long>(
                    name: "CorrectCount",
                    table: GetShardingTableName<Question>(),
                    type: "bigint",
                    nullable: false,
                    defaultValue: 0L,
                    comment: "答对次数");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AddColumn<long>(
                    name: "WrongCount",
                    table: GetShardingTableName<Question>(),
                    type: "bigint",
                    nullable: false,
                    defaultValue: 0L,
                    comment: "答错次数");
            }

            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Question_SubjectId_ChapterNodeId_QuestionCategoryId_TenantId",
                    table: GetShardingTableName<Question>(),
                    columns: new[] { "SubjectId", "QuestionCategoryId", "TenantId" });
            }

        }
    }
}
