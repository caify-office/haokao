using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionCategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionCategoryService.Infrastructure.Migrations
{
    public partial class adaptPlaceField : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Name",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "类名名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldComment: "类名名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Code",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "类别代码",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldComment: "类别代码")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AddColumn<long>(
                    name: "AdaptPlace",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "bigint",
                    nullable: false,
                    defaultValue: 0L);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.DropColumn(
                    name: "AdaptPlace",
                    table: GetShardingTableName<QuestionCategory>());
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<QuestionCategory>(),
                    keyColumn: "Name",
                    keyValue: null,
                    column: "Name",
                    value: "");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Name",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: false,
                    comment: "类名名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "类名名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<QuestionCategory>(),
                    keyColumn: "Code",
                    keyValue: null,
                    column: "Code",
                    value: "");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Code",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: false,
                    comment: "类别代码",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "类别代码")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
