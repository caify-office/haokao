using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class addCourseType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "CourseType",
                    table: GetShardingTableName<Course>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.DropColumn(
                    name: "CourseType",
                    table: GetShardingTableName<Course>());
            }

        }
    }
}
