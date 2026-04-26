using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class useLongText : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "TextAnalysis",
                    table: GetShardingTableName<Question>(),
                    type: "longtext",
                    nullable: true,
                    comment: "文字解析",
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldNullable: true,
                    oldComment: "文字解析")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "QuestionText",
                    table: GetShardingTableName<Question>(),
                    type: "longtext",
                    nullable: false,
                    comment: "试题内容 (题干)",
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldComment: "试题内容 (题干)")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "TextAnalysis",
                    table: GetShardingTableName<Question>(),
                    type: "text",
                    nullable: true,
                    comment: "文字解析",
                    oldClrType: typeof(string),
                    oldType: "longtext",
                    oldNullable: true,
                    oldComment: "文字解析")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Question>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "QuestionText",
                    table: GetShardingTableName<Question>(),
                    type: "text",
                    nullable: false,
                    comment: "试题内容 (题干)",
                    oldClrType: typeof(string),
                    oldType: "longtext",
                    oldComment: "试题内容 (题干)")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
