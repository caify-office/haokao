using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Migrations
{
    public partial class InitNewAnswerRecord : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AnswerRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<AnswerRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        QuestionCount = table.Column<int>(type: "int", nullable: false, comment: "题目数量"),
                        AnswerCount = table.Column<int>(type: "int", nullable: false, comment: "答题数量"),
                        CorrectCount = table.Column<int>(type: "int", nullable: false, comment: "正确数量"),
                        ElapsedTime = table.Column<long>(type: "bigint", nullable: false, comment: "答题时间"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "答题时间"),
                        AnswerType = table.Column<int>(type: "int", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_AnswerRecord", x => x.Id);
                    },
                    comment: "作答记录")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<AnswerQuestion>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<AnswerQuestion>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        RecordId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "作答记录Id", collation: "ascii_general_ci"),
                        QuestionId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "试题Id", collation: "ascii_general_ci"),
                        QuestionTypeId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "试题类型Id", collation: "ascii_general_ci"),
                        UserAnswers = table.Column<string>(type: "json", nullable: false, comment: "用户作答")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        JudgeResult = table.Column<int>(type: "int", nullable: false, comment: "判题结果"),
                        WhetherMark = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否标记"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "答题时间"),
                        ParentId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "试题父Id", collation: "ascii_general_ci"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_AnswerQuestion", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<AnswerQuestion>("FK_AnswerQuestion_AnswerRecord_RecordId"),
                            column: x => x.RecordId,
                            principalTable: GetShardingTableName<AnswerRecord>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    },
                    comment: "作答试题")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ChapterAnswerRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ChapterAnswerRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "分类Id", collation: "ascii_general_ci"),
                        ChapterId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "章节Id", collation: "ascii_general_ci"),
                        SectionId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "小节Id", collation: "ascii_general_ci"),
                        KnowledgePointId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "知识点Id", collation: "ascii_general_ci"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "答题时间"),
                        AnswerRecordId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "作答记录Id", collation: "ascii_general_ci"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ChapterAnswerRecord", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<ChapterAnswerRecord>("FK_ChapterAnswerRecord_AnswerRecord_AnswerRecordId"),
                            column: x => x.AnswerRecordId,
                            principalTable: GetShardingTableName<AnswerRecord>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    },
                    comment: "章节知识点作答记录")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<DateAnswerRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<DateAnswerRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        Type = table.Column<int>(type: "int", nullable: false, comment: "类型"),
                        Date = table.Column<DateOnly>(type: "date", nullable: false, comment: "日期"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "答题时间"),
                        AnswerRecordId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "答题记录Id", collation: "ascii_general_ci"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_DateAnswerRecord", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<DateAnswerRecord>("FK_DateAnswerRecord_AnswerRecord_AnswerRecordId"),
                            column: x => x.AnswerRecordId,
                            principalTable: GetShardingTableName<AnswerRecord>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    },
                    comment: "日期相关作答记录")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<PaperAnswerRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<PaperAnswerRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "类别Id", collation: "ascii_general_ci"),
                        PaperId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "试卷Id", collation: "ascii_general_ci"),
                        UserScore = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false, comment: "用户得分"),
                        PassingScore = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false, comment: "及格分"),
                        TotalScore = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false, comment: "总分"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "答题时间"),
                        AnswerRecordId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "答题记录Id", collation: "ascii_general_ci"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_PaperAnswerRecord", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<PaperAnswerRecord>("FK_PaperAnswerRecord_AnswerRecord_AnswerRecordId"),
                            column: x => x.AnswerRecordId,
                            principalTable: GetShardingTableName<AnswerRecord>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    },
                    comment: "试卷作答记录")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<AnswerQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AnswerQuestion_QuestionId_CreatorId_CreateTime_TenantId",
                    table: GetShardingTableName<AnswerQuestion>(),
                    columns: new[] { "QuestionId", "CreatorId", "CreateTime", "TenantId" });
            }


            if (IsCreateShardingTable<AnswerQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AnswerQuestion_RecordId",
                    table: GetShardingTableName<AnswerQuestion>(),
                    column: "RecordId");
            }


            if (IsCreateShardingTable<AnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AnswerRecord_SubjectId_CreatorId_TenantId",
                    table: GetShardingTableName<AnswerRecord>(),
                    columns: new[] { "SubjectId", "CreatorId", "TenantId" });
            }


            if (IsCreateShardingTable<ChapterAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ChapterAnswerRecord_AnswerRecordId",
                    table: GetShardingTableName<ChapterAnswerRecord>(),
                    column: "AnswerRecordId",
                    unique: true);
            }


            if (IsCreateShardingTable<ChapterAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ChapterAnswerRecord_KnowledgePointId_CreatorId_CategoryId_Te~",
                    table: GetShardingTableName<ChapterAnswerRecord>(),
                    columns: new[] { "KnowledgePointId", "CreatorId", "CategoryId", "TenantId" });
            }


            if (IsCreateShardingTable<ChapterAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ChapterAnswerRecord_KnowledgePointId_SectionId_ChapterId_Sub~",
                    table: GetShardingTableName<ChapterAnswerRecord>(),
                    columns: new[] { "KnowledgePointId", "SectionId", "ChapterId", "SubjectId", "CreatorId", "CategoryId", "TenantId" });
            }


            if (IsCreateShardingTable<ChapterAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ChapterAnswerRecord_SectionId_CreatorId_CategoryId_TenantId",
                    table: GetShardingTableName<ChapterAnswerRecord>(),
                    columns: new[] { "SectionId", "CreatorId", "CategoryId", "TenantId" });
            }


            if (IsCreateShardingTable<ChapterAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ChapterAnswerRecord_SubjectId_CreatorId_CategoryId_TenantId",
                    table: GetShardingTableName<ChapterAnswerRecord>(),
                    columns: new[] { "SubjectId", "CreatorId", "CategoryId", "TenantId" });
            }


            if (IsCreateShardingTable<DateAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_DateAnswerRecord_AnswerRecordId",
                    table: GetShardingTableName<DateAnswerRecord>(),
                    column: "AnswerRecordId",
                    unique: true);
            }


            if (IsCreateShardingTable<DateAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_DateAnswerRecord_Date_SubjectId_Type_CreatorId_TenantId",
                    table: GetShardingTableName<DateAnswerRecord>(),
                    columns: new[] { "Date", "SubjectId", "Type", "CreatorId", "TenantId" },
                    unique: true);
            }


            if (IsCreateShardingTable<PaperAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_PaperAnswerRecord_AnswerRecordId",
                    table: GetShardingTableName<PaperAnswerRecord>(),
                    column: "AnswerRecordId",
                    unique: true);
            }


            if (IsCreateShardingTable<PaperAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_PaperAnswerRecord_PaperId_SubjectId_CreatorId_CategoryId_Ten~",
                    table: GetShardingTableName<PaperAnswerRecord>(),
                    columns: new[] { "PaperId", "SubjectId", "CreatorId", "CategoryId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AnswerQuestion>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<AnswerQuestion>());
            }


            if (IsCreateShardingTable<ChapterAnswerRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ChapterAnswerRecord>());
            }


            if (IsCreateShardingTable<DateAnswerRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<DateAnswerRecord>());
            }


            if (IsCreateShardingTable<PaperAnswerRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<PaperAnswerRecord>());
            }


            if (IsCreateShardingTable<AnswerRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<AnswerRecord>());
            }

        }
    }
}
