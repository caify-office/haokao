using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.AnsweringQuestionService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.AnsweringQuestionService.Infrastructure.Migrations
{
    public partial class alterFileUrlLength : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AnsweringQuestion>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "FileUrl",
                    table: GetShardingTableName<AnsweringQuestion>(),
                    type: "text",
                    nullable: true,
                    comment: "上传的图片路径",
                    oldClrType: typeof(string),
                    oldType: "varchar(2000)",
                    oldMaxLength: 2000,
                    oldNullable: true,
                    oldComment: "上传的图片路劲")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AnsweringQuestion>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "FileUrl",
                    table: GetShardingTableName<AnsweringQuestion>(),
                    type: "varchar(2000)",
                    maxLength: 2000,
                    nullable: true,
                    comment: "上传的图片路劲",
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldNullable: true,
                    oldComment: "上传的图片路径")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
