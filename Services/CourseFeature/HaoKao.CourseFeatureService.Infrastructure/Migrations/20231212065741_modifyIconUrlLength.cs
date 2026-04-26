using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseFeatureService.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseFeatureService.Infrastructure.Migrations
{
    public partial class modifyIconUrlLength : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseFeature>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "IconUrl",
                    table: GetShardingTableName<CourseFeature>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: false,
                    comment: "图标地址",
                    oldClrType: typeof(string),
                    oldType: "varchar(100)",
                    oldMaxLength: 100,
                    oldComment: "图标地址")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseFeature>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "IconUrl",
                    table: GetShardingTableName<CourseFeature>(),
                    type: "varchar(100)",
                    maxLength: 100,
                    nullable: false,
                    comment: "图标地址",
                    oldClrType: typeof(string),
                    oldType: "varchar(500)",
                    oldMaxLength: 500,
                    oldComment: "图标地址")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}