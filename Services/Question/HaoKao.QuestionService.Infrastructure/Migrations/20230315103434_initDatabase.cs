using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class initDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Question>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        SubjectName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "科目名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ChapterNodeId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "章节Id", collation: "ascii_general_ci"),
                        ChapterNodeName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "章节名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        QuestionCategoryId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "试题分类Id", collation: "ascii_general_ci"),
                        QuestionCategoryName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "试题分类名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        QuestionTypeId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "试题类型Id", collation: "ascii_general_ci"),
                        QuestionTypeName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "试题类型名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        QuestionText = table.Column<string>(type: "text", nullable: false, comment: "试题内容 (题干)")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TextAnalysis = table.Column<string>(type: "text", nullable: true, comment: "文字解析")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        MediaAnalysis = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "音视频解析")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AbilityIds = table.Column<string>(type: "varchar(360)", maxLength: 360, nullable: true, comment: "能力维度id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        FreeState = table.Column<int>(type: "int", nullable: true, comment: "免费分区"),
                        EnableState = table.Column<int>(type: "int", nullable: false, comment: "启用状态"),
                        Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                        CorrectCount = table.Column<long>(type: "bigint", nullable: false, comment: "答对次数"),
                        WrongCount = table.Column<long>(type: "bigint", nullable: false, comment: "答错次数"),
                        QuestionOptions = table.Column<string>(type: "json", nullable: true, comment: "试题选项")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ParentId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "父题目Id", collation: "ascii_general_ci"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Question", x => x.Id);
                    },
                    comment: "试题")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Question_SubjectId_ChapterNodeId_TenantId",
                    table: GetShardingTableName<Question>(),
                    columns: new[] { "SubjectId", "ChapterNodeId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Question>());
            }

        }
    }
}
