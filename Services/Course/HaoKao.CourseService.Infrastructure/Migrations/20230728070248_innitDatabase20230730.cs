using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseMaterialsModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class innitDatabase20230730 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "Sort",
                    table: GetShardingTableName<CourseMaterials>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.DropColumn(
                    name: "Sort",
                    table: GetShardingTableName<CourseMaterials>());
            }

        }
    }
}
