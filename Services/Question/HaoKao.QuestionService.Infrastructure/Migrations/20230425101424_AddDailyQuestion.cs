using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.DailyQuestionModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class AddDailyQuestion : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DailyQuestion>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<DailyQuestion>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                        QuestionId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "试题Id", collation: "ascii_general_ci"),
                        CreateDate = table.Column<string>(type: "varchar(255)", nullable: true, comment: "创建日期")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_DailyQuestion", x => x.Id);
                    },
                    comment: "每日一提")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<DailyQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_DailyQuestion_QuestionId_CreateDate_TenantId",
                    table: GetShardingTableName<DailyQuestion>(),
                    columns: new[] { "QuestionId", "CreateDate", "TenantId" },
                    unique: true);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DailyQuestion>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<DailyQuestion>());
            }

        }
    }
}