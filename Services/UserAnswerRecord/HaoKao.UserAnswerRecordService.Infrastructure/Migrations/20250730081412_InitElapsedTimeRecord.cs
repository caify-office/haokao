using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Migrations
{
    public partial class InitElapsedTimeRecord : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AnswerRecord>())
            {
                migrationBuilder.DropColumn(
                    name: "ElapsedTime",
                    table: GetShardingTableName<AnswerRecord>());
            }


            if (IsCreateShardingTable<PaperAnswerRecord>())
            {
                migrationBuilder.AddColumn<long>(
                    name: "ElapsedTime",
                    table: GetShardingTableName<PaperAnswerRecord>(),
                    type: "bigint",
                    nullable: false,
                    defaultValue: 0L,
                    comment: "答题时间");
            }


            if (IsCreateShardingTable<AnswerRecord>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "QuestionCount",
                    table: GetShardingTableName<AnswerRecord>(),
                    type: "int",
                    nullable: false,
                    comment: "总题数",
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldComment: "题目数量");
            }


            if (IsCreateShardingTable<AnswerRecord>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "CorrectCount",
                    table: GetShardingTableName<AnswerRecord>(),
                    type: "int",
                    nullable: false,
                    comment: "正确数",
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldComment: "正确数量");
            }


            if (IsCreateShardingTable<AnswerRecord>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "AnswerCount",
                    table: GetShardingTableName<AnswerRecord>(),
                    type: "int",
                    nullable: false,
                    comment: "答题数",
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldComment: "答题数量");
            }


            if (IsCreateShardingTable<ElapsedTimeRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ElapsedTimeRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        TargetId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "目标Id", collation: "ascii_general_ci"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "产品Id", collation: "ascii_general_ci"),
                        Type = table.Column<int>(type: "int", nullable: false, comment: "类型"),
                        QuestionCount = table.Column<int>(type: "int", nullable: false, comment: "总题数"),
                        AnswerCount = table.Column<int>(type: "int", nullable: false, comment: "答题数"),
                        CorrectCount = table.Column<int>(type: "int", nullable: false, comment: "正确数"),
                        ElapsedSeconds = table.Column<long>(type: "bigint", nullable: false, comment: "耗时(秒)"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "做题时间"),
                        CreateDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "做题日期"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ElapsedTimeRecord", x => x.Id);
                    },
                    comment: "用户做题时长")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ElapsedTimeRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ElapsedTimeRecord_CreatorId_SubjectId_Type_ProductId_TenantId",
                    table: GetShardingTableName<ElapsedTimeRecord>(),
                    columns: new[] { "CreatorId", "SubjectId", "Type", "ProductId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ElapsedTimeRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ElapsedTimeRecord>());
            }


            if (IsCreateShardingTable<PaperAnswerRecord>())
            {
                migrationBuilder.DropColumn(
                    name: "ElapsedTime",
                    table: GetShardingTableName<PaperAnswerRecord>());
            }


            if (IsCreateShardingTable<AnswerRecord>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "QuestionCount",
                    table: GetShardingTableName<AnswerRecord>(),
                    type: "int",
                    nullable: false,
                    comment: "题目数量",
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldComment: "总题数");
            }


            if (IsCreateShardingTable<AnswerRecord>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "CorrectCount",
                    table: GetShardingTableName<AnswerRecord>(),
                    type: "int",
                    nullable: false,
                    comment: "正确数量",
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldComment: "正确数");
            }


            if (IsCreateShardingTable<AnswerRecord>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "AnswerCount",
                    table: GetShardingTableName<AnswerRecord>(),
                    type: "int",
                    nullable: false,
                    comment: "答题数量",
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldComment: "答题数");
            }


            if (IsCreateShardingTable<AnswerRecord>())
            {
                migrationBuilder.AddColumn<long>(
                    name: "ElapsedTime",
                    table: GetShardingTableName<AnswerRecord>(),
                    type: "bigint",
                    nullable: false,
                    defaultValue: 0L,
                    comment: "答题时间");
            }

        }
    }
}
