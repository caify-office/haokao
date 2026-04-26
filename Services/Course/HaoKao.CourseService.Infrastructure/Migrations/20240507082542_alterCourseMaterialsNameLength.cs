using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseMaterialsModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class alterCourseMaterialsNameLength : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Name",
                    table: GetShardingTableName<CourseMaterials>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "讲义名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "讲义名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Name",
                    table: GetShardingTableName<CourseMaterials>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "讲义名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(500)",
                    oldMaxLength: 500,
                    oldNullable: true,
                    oldComment: "讲义名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
