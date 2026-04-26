using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LearnProgressService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearnProgressService.Infrastructure.Migrations
{
    public partial class alterStudyDurationTYpe : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DailyStudyDuration>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "DailyVideoStudyDuration",
                    table: GetShardingTableName<DailyStudyDuration>(),
                    type: "decimal(6,2)",
                    nullable: false,
                    comment: "今日学习时长",
                    oldClrType: typeof(double),
                    oldType: "double");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DailyStudyDuration>())
            {
                migrationBuilder.AlterColumn<double>(
                    name: "DailyVideoStudyDuration",
                    table: GetShardingTableName<DailyStudyDuration>(),
                    type: "double",
                    nullable: false,
                    oldClrType: typeof(decimal),
                    oldType: "decimal(6,2)",
                    oldComment: "今日学习时长");
            }

        }
    }
}
