using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CoursePracticeModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class addSubjectId : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "SubjectId",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropColumn(
                    name: "SubjectId",
                    table: GetShardingTableName<CoursePractice>());
            }

        }
    }
}
