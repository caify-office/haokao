using System;
using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearningPlanService.Infrastructure.Migrations
{
    public partial class addInitData : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<LearningPlan>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LearningPlan>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "对应科目名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        EndDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "学习计划结束日期"),
                        DayLearningTimes = table.Column<string>(type: "json", nullable: true, comment: "每周学习时长配置(分钟)")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        NeedReminder = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        ReminderHours = table.Column<int>(type: "int", nullable: false),
                        ReminderMinutes = table.Column<int>(type: "int", nullable: false),
                        ReminderPhone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "提醒手机号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LearningPlan", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LearningTask>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LearningTask>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        TaskName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "任务名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ScheduledTime = table.Column<DateOnly>(type: "date", nullable: false, comment: "计划开始该任务的时间点"),
                        DurationSeconds = table.Column<decimal>(type: "decimal(7,2)", maxLength: 10, nullable: false, comment: "完成该任务预计需要的时长（秒），支持小数表示"),
                        TaskType = table.Column<int>(type: "int", nullable: false),
                        TaskIdentity = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "任务标识（用于重新制作学习计划时判定当前任务内容是否已是过期任务内容）")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TaskContent = table.Column<string>(type: "json", nullable: true, comment: "任务内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        LearningPlanId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Sort = table.Column<int>(type: "int", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LearningTask", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<LearningTask>("FK_LearningTask_LearningPlan_LearningPlanId"),
                            column: x => x.LearningPlanId,
                            principalTable: GetShardingTableName<LearningPlan>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LearningTask>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LearningTask_LearningPlanId",
                    table: GetShardingTableName<LearningTask>(),
                    column: "LearningPlanId");
            }


            if (IsCreateShardingTable<LearningTask>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LearningTask_ScheduledTime_LearningPlanId",
                    table: GetShardingTableName<LearningTask>(),
                    columns: new[] { "ScheduledTime", "LearningPlanId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearningTask>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LearningTask>());
            }


            if (IsCreateShardingTable<LearningPlan>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LearningPlan>());
            }

        }
    }
}
