using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class addCourseMaterialsPackageUrl : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "CourseMaterialsPackageUrl",
                    table: GetShardingTableName<Course>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "课程讲义包url")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.DropColumn(
                    name: "CourseMaterialsPackageUrl",
                    table: GetShardingTableName<Course>());
            }

        }
    }
}
