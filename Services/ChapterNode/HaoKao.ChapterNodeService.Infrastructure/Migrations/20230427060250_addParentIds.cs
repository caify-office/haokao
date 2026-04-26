using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ChapterNodeService.Infrastructure.Migrations
{
    public partial class addParentIds : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "ParentName",
                    table: GetShardingTableName<ChapterNode>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "父级名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldComment: "父级名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Name",
                    table: GetShardingTableName<ChapterNode>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "章节名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldComment: "章节名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Code",
                    table: GetShardingTableName<ChapterNode>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "编码",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldComment: "编码")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "ParentIds",
                    table: GetShardingTableName<ChapterNode>(),
                    type: "longtext",
                    nullable: true,
                    comment: "父级Id集合")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.DropColumn(
                    name: "ParentIds",
                    table: GetShardingTableName<ChapterNode>());
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<ChapterNode>(),
                    keyColumn: "ParentName",
                    keyValue: null,
                    column: "ParentName",
                    value: "");
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "ParentName",
                    table: GetShardingTableName<ChapterNode>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: false,
                    comment: "父级名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "父级名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<ChapterNode>(),
                    keyColumn: "Name",
                    keyValue: null,
                    column: "Name",
                    value: "");
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Name",
                    table: GetShardingTableName<ChapterNode>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: false,
                    comment: "章节名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "章节名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<ChapterNode>(),
                    keyColumn: "Code",
                    keyValue: null,
                    column: "Code",
                    value: "");
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Code",
                    table: GetShardingTableName<ChapterNode>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: false,
                    comment: "编码",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "编码")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
