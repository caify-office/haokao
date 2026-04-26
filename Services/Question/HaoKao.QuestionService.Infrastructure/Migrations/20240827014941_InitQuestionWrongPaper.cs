using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionWrongPaperMoudle;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class InitQuestionWrongPaper : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionWrongPaper>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<QuestionWrongPaper>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        PaperName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "试卷名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        DownloadUrl = table.Column<string>(type: "text", nullable: false, comment: "下载地址")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "创建人Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_QuestionWrongPaper", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<QuestionWrongPaper>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_QuestionWrongPaper_SubjectId_CreatorId_CreateTime_TenantId",
                    table: GetShardingTableName<QuestionWrongPaper>(),
                    columns: new[] { "SubjectId", "CreatorId", "CreateTime", "TenantId" },
                    unique: true);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionWrongPaper>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<QuestionWrongPaper>());
            }

        }
    }
}
