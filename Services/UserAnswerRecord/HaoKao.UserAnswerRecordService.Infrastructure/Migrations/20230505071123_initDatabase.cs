using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Migrations
{
    public partial class initDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<UserAnswerRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<UserAnswerRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        AnswerType = table.Column<int>(type: "int", nullable: false, comment: "答题类型"),
                        RecordIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, comment: "答题标识符 章节Id，或试卷Id，每日一练为Guid.Empty", collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        QuestionCategoryId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "分类Id", collation: "ascii_general_ci"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        UserScore = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false, comment: "用户得分"),
                        PassingScore = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false, comment: "及格分数"),
                        TotalScore = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false, comment: "试题总分"),
                        ElapsedTime = table.Column<long>(type: "bigint", nullable: false, comment: "耗时"),
                        QuestionCount = table.Column<int>(type: "int", nullable: false, comment: "总题数"),
                        AnswerCount = table.Column<int>(type: "int", nullable: false, comment: "作答数"),
                        CorrectCount = table.Column<int>(type: "int", nullable: false, comment: "正确数"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_UserAnswerRecord_2023", x => x.Id);
                    },
                    comment: "答题记录")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<UserAnswerQuestion>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        AnswerRecordId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "答题记录Id", collation: "ascii_general_ci"),
                        QuestionId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "试题Id", collation: "ascii_general_ci"),
                        QuestionTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        UserScore = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false, comment: "用户得分"),
                        JudgeResult = table.Column<int>(type: "int", nullable: false, comment: "判题结果"),
                        WhetherMark = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        UserAnswerRecordId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_UserAnswerQuestion_2023", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<UserAnswerQuestion>("FK_UserAnswerQuestion_2023_UserAnswerRecord_2023_UserAnswerReco~"),
                            column: x => x.UserAnswerRecordId,
                            principalTable: GetShardingTableName<UserAnswerRecord>(),
                            principalColumn: "Id");
                    },
                    comment: "答题记录试题表")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserQuestionOption>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<UserQuestionOption>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        OptionId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "选项Id", collation: "ascii_general_ci"),
                        OptionContent = table.Column<string>(type: "text", nullable: true, comment: "回答内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        UserAnswerQuestionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_UserQuestionOption_2023", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<UserQuestionOption>("FK_UserQuestionOption_2023_UserAnswerQuestion_2023_UserAnswerQu~"),
                            column: x => x.UserAnswerQuestionId,
                            principalTable: GetShardingTableName<UserAnswerQuestion>(),
                            principalColumn: "Id");
                    },
                    comment: "用户答题的选项或内容")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UserAnswerQuestion_2023_AnswerRecordId_QuestionId_TenantId",
                    table: GetShardingTableName<UserAnswerQuestion>(),
                    columns: new[] { "AnswerRecordId", "QuestionId", "TenantId" });
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UserAnswerQuestion_2023_UserAnswerRecordId",
                    table: GetShardingTableName<UserAnswerQuestion>(),
                    column: "UserAnswerRecordId");
            }


            if (IsCreateShardingTable<UserAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UserAnswerRecord_2023_SubjectId_QuestionCategoryId_RecordIde~",
                    table: GetShardingTableName<UserAnswerRecord>(),
                    columns: new[] { "SubjectId", "QuestionCategoryId", "RecordIdentifier", "CreatorId", "TenantId" });
            }


            if (IsCreateShardingTable<UserQuestionOption>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UserQuestionOption_2023_UserAnswerQuestionId",
                    table: GetShardingTableName<UserQuestionOption>(),
                    column: "UserAnswerQuestionId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserQuestionOption>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<UserQuestionOption>());
            }


            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<UserAnswerQuestion>());
            }


            if (IsCreateShardingTable<UserAnswerRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<UserAnswerRecord>());
            }

        }
    }
}