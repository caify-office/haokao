using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionCategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionCategoryService.Infrastructure.Migrations
{
    public partial class AddDisplayCondition : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "DisplayCondition",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 1,
                    comment: "显示条件",
                    oldClrType: typeof(int),
                    oldType: "int");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "DisplayCondition",
                    table: GetShardingTableName<QuestionCategory>(),
                    type: "int",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldDefaultValue: 1,
                    oldComment: "显示条件");
            }

        }
    }
}
