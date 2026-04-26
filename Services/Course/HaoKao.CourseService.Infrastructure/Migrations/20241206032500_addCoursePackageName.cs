using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class addCoursePackageName : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "CourseMaterialsPackageName",
                    table: GetShardingTableName<Course>(),
                    type: "varchar(100)",
                    maxLength: 100,
                    nullable: true,
                    comment: "课程讲义包名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.DropColumn(
                    name: "CourseMaterialsPackageName",
                    table: GetShardingTableName<Course>());
            }

        }
    }
}
