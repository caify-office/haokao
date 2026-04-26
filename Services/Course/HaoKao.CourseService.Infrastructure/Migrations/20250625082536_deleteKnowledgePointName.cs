using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseMaterialsModule;
using HaoKao.CourseService.Domain.CoursePracticeModule;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class deleteKnowledgePointName : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropColumn(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CourseVideo>());
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropColumn(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CoursePractice>());
            }


            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.DropColumn(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CourseMaterials>());
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "关联的知识点名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "关联的知识点名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CourseMaterials>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "关联的知识点名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
