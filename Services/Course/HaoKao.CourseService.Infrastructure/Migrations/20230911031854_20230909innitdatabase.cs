using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class _20230909innitdatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "QzName",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "前缀名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropColumn(
                    name: "QzName",
                    table: GetShardingTableName<CourseVideo>());
            }

        }
    }
}
