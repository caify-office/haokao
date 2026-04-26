using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionCategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionCategoryService.Infrastructure.Migrations
{
    public partial class AddProductPackage : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "ProductPackageType",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "int",
                    nullable: true,
                    comment: "产品包类型",
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldNullable: true);
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "ProductPackageId",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    comment: "产品包Id(购买跳转对象)",
                    collation: "ascii_general_ci",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)")
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "ProductPackageType",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "int",
                    nullable: true,
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldNullable: true,
                    oldComment: "产品包类型");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "ProductPackageId",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "char(36)",
                    nullable: false,
                    collation: "ascii_general_ci",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)",
                    oldDefaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    oldComment: "产品包Id(购买跳转对象)")
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }

        }
    }
}
