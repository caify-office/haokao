using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.DailyQuestionModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class modifyDailyDateField : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DailyQuestion>())
            {
                migrationBuilder.AlterColumn<DateTime>(
                    name: "CreateDate",
                    table: GetShardingTableName<DailyQuestion>(),
                    type: "datetime(6)",
                    nullable: false,
                    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    comment: "创建日期",
                    oldClrType: typeof(string),
                    oldType: "varchar(255)",
                    oldNullable: true,
                    oldComment: "创建日期")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DailyQuestion>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreateDate",
                    table: GetShardingTableName<DailyQuestion>(),
                    type: "varchar(255)",
                    nullable: true,
                    comment: "创建日期",
                    oldClrType: typeof(DateTime),
                    oldType: "datetime(6)",
                    oldComment: "创建日期")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
