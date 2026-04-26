using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseMaterialsModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class innitDatabase20230726 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "CourseChapterId",
                    table: GetShardingTableName<CourseMaterials>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.DropColumn(
                    name: "CourseChapterId",
                    table: GetShardingTableName<CourseMaterials>());
            }

        }
    }
}
