using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CoursePracticeModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class AddExamFrequencyToCoursePractice : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "ExamFrequency",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropColumn(
                    name: "ExamFrequency",
                    table: GetShardingTableName<CoursePractice>());
            }

        }
    }
}
