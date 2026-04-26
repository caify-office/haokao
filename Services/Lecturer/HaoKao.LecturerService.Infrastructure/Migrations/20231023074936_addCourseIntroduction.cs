using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LecturerService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LecturerService.Infrastructure.Migrations
{
    public partial class addCourseIntroduction : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "CourseIntroduction",
                    table: GetShardingTableName<Lecturer>(),
                    type: "text",
                    nullable: true,
                    comment: "课程介绍")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.DropColumn(
                    name: "CourseIntroduction",
                    table: GetShardingTableName<Lecturer>());
            }

        }
    }
}
