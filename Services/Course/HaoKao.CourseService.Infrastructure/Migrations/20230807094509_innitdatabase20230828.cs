using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class innitdatabase20230828 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "StudyMatrialId",
                    table: GetShardingTableName<Course>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "学习资料")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.DropColumn(
                    name: "StudyMatrialId",
                    table: GetShardingTableName<Course>());
            }

        }
    }
}
