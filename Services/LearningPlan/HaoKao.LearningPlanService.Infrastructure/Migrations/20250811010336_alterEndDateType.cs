using System;
using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearningPlanService.Infrastructure.Migrations
{
    public partial class alterEndDateType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearningPlan>())
            {
                migrationBuilder.AlterColumn<DateOnly>(
                    name: "EndDate",
                    table: GetShardingTableName<LearningPlan>(),
                    type: "date",
                    nullable: false,
                    comment: "学习计划结束日期",
                    oldClrType: typeof(DateTime),
                    oldType: "datetime",
                    oldComment: "学习计划结束日期");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearningPlan>())
            {
                migrationBuilder.AlterColumn<DateTime>(
                    name: "EndDate",
                    table: GetShardingTableName<LearningPlan>(),
                    type: "datetime",
                    nullable: false,
                    comment: "学习计划结束日期",
                    oldClrType: typeof(DateOnly),
                    oldType: "date",
                    oldComment: "学习计划结束日期");
            }

        }
    }
}
