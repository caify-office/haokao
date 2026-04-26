using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionCategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionCategoryService.Infrastructure.Migrations
{
    public partial class AddColumnComment : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "TenantId",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "varchar(36)",
                    nullable: false,
                    comment: "租户Id",
                    oldClrType: typeof(string),
                    oldType: "varchar(36)")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<DateTime>(
                    name: "CreateTime",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "datetime",
                    nullable: false,
                    comment: "创建时间",
                    oldClrType: typeof(DateTime),
                    oldType: "datetime");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<long>(
                    name: "AdaptPlace",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "bigint",
                    nullable: false,
                    comment: "适应场景",
                    oldClrType: typeof(long),
                    oldType: "bigint");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "DisplayCondition",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "ProductPackageId",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "ProductPackageType",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "int",
                    nullable: true);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.DropColumn(
                    name: "DisplayCondition",
                    table: GetShardingTableName<QuestionCategory>());
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.DropColumn(
                    name: "ProductPackageId",
                    table: GetShardingTableName<QuestionCategory>());
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.DropColumn(
                    name: "ProductPackageType",
                    table: GetShardingTableName<QuestionCategory>());
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "TenantId",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "varchar(36)",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(36)",
                    oldComment: "租户Id")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<DateTime>(
                    name: "CreateTime",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "datetime",
                    nullable: false,
                    oldClrType: typeof(DateTime),
                    oldType: "datetime",
                    oldComment: "创建时间");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<long>(
                    name: "AdaptPlace",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "bigint",
                    nullable: false,
                    oldClrType: typeof(long),
                    oldType: "bigint",
                    oldComment: "适应场景");
            }

        }
    }
}
