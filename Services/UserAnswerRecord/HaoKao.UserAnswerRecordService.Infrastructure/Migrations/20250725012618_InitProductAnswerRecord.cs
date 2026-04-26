using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Migrations
{
    public partial class InitProductAnswerRecord : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ProductChapterAnswerRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ProductChapterAnswerRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "产品Id", collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        ChapterId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "章节Id", collation: "ascii_general_ci"),
                        SectionId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "小节Id", collation: "ascii_general_ci"),
                        KnowledgePointId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "知识点Id", collation: "ascii_general_ci"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "答题时间"),
                        CreateDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "答题日期"),
                        AnswerRecordId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "答题记录Id", collation: "ascii_general_ci"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ProductChapterAnswerRecord", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<ProductChapterAnswerRecord>("FK_ProductChapterAnswerRecord_AnswerRecord_AnswerRecordId"),
                            column: x => x.AnswerRecordId,
                            principalTable: GetShardingTableName<AnswerRecord>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ProductKnowledgeAnswerRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ProductKnowledgeAnswerRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "产品Id", collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        ChapterId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "章节Id", collation: "ascii_general_ci"),
                        SectionId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "小节Id", collation: "ascii_general_ci"),
                        KnowledgePointId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "知识点Id", collation: "ascii_general_ci"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "答题时间"),
                        CreateDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "答题日期"),
                        CorrectRate = table.Column<int>(type: "int", nullable: false, comment: "正确率"),
                        MasteryLevel = table.Column<int>(type: "int", nullable: false, comment: "掌握程度"),
                        ExamFrequency = table.Column<int>(type: "int", nullable: false, comment: "考频"),
                        AnswerRecordId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "作答记录Id", collation: "ascii_general_ci"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ProductKnowledgeAnswerRecord", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<ProductKnowledgeAnswerRecord>("FK_ProductKnowledgeAnswerRecord_AnswerRecord_AnswerRecordId"),
                            column: x => x.AnswerRecordId,
                            principalTable: GetShardingTableName<AnswerRecord>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    },
                    comment: "产品知识点作答记录")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ProductPaperAnswerRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ProductPaperAnswerRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "产品Id", collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        PaperId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "试卷Id", collation: "ascii_general_ci"),
                        UserScore = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false, comment: "用户得分"),
                        PassingScore = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false, comment: "及格分"),
                        TotalScore = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false, comment: "总分"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "答题时间"),
                        CreateDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "答题日期"),
                        AnswerRecordId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "答题记录Id", collation: "ascii_general_ci"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ProductPaperAnswerRecord", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<ProductPaperAnswerRecord>("FK_ProductPaperAnswerRecord_AnswerRecord_AnswerRecordId"),
                            column: x => x.AnswerRecordId,
                            principalTable: GetShardingTableName<AnswerRecord>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    },
                    comment: "产品试卷作答记录")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ProductChapterAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ProductChapterAnswerRecord_AnswerRecordId",
                    table: GetShardingTableName<ProductChapterAnswerRecord>(),
                    column: "AnswerRecordId",
                    unique: true);
            }


            if (IsCreateShardingTable<ProductChapterAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ProductChapterAnswerRecord_ChapterId_CreatorId_ProductId_Ten~",
                    table: GetShardingTableName<ProductChapterAnswerRecord>(),
                    columns: new[] { "ChapterId", "CreatorId", "ProductId", "TenantId" });
            }


            if (IsCreateShardingTable<ProductChapterAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ProductChapterAnswerRecord_ChapterId_SubjectId_CreatorId_Pro~",
                    table: GetShardingTableName<ProductChapterAnswerRecord>(),
                    columns: new[] { "ChapterId", "SubjectId", "CreatorId", "ProductId", "TenantId" });
            }


            if (IsCreateShardingTable<ProductKnowledgeAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ProductKnowledgeAnswerRecord_AnswerRecordId",
                    table: GetShardingTableName<ProductKnowledgeAnswerRecord>(),
                    column: "AnswerRecordId",
                    unique: true);
            }


            if (IsCreateShardingTable<ProductKnowledgeAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ProductKnowledgeAnswerRecord_KnowledgePointId_CreatorId_Prod~",
                    table: GetShardingTableName<ProductKnowledgeAnswerRecord>(),
                    columns: new[] { "KnowledgePointId", "CreatorId", "ProductId", "TenantId" });
            }


            if (IsCreateShardingTable<ProductKnowledgeAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ProductKnowledgeAnswerRecord_KnowledgePointId_SubjectId_Crea~",
                    table: GetShardingTableName<ProductKnowledgeAnswerRecord>(),
                    columns: new[] { "KnowledgePointId", "SubjectId", "CreatorId", "ProductId", "TenantId" });
            }


            if (IsCreateShardingTable<ProductPaperAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ProductPaperAnswerRecord_AnswerRecordId",
                    table: GetShardingTableName<ProductPaperAnswerRecord>(),
                    column: "AnswerRecordId",
                    unique: true);
            }


            if (IsCreateShardingTable<ProductPaperAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ProductPaperAnswerRecord_PaperId_CreatorId_ProductId_TenantId",
                    table: GetShardingTableName<ProductPaperAnswerRecord>(),
                    columns: new[] { "PaperId", "CreatorId", "ProductId", "TenantId" });
            }


            if (IsCreateShardingTable<ProductPaperAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ProductPaperAnswerRecord_PaperId_SubjectId_CreatorId_Product~",
                    table: GetShardingTableName<ProductPaperAnswerRecord>(),
                    columns: new[] { "PaperId", "SubjectId", "CreatorId", "ProductId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ProductChapterAnswerRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ProductChapterAnswerRecord>());
            }


            if (IsCreateShardingTable<ProductKnowledgeAnswerRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ProductKnowledgeAnswerRecord>());
            }


            if (IsCreateShardingTable<ProductPaperAnswerRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ProductPaperAnswerRecord>());
            }

        }
    }
}
