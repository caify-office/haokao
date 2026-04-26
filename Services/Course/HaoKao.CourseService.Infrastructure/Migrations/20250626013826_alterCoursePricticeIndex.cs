using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CoursePracticeModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class alterCoursePricticeIndex : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_CoursePractice_CourseChapterId",
                    table: GetShardingTableName<CoursePractice>());
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_CoursePractice_CourseChapterId_KnowledgePointId",
                    table: GetShardingTableName<CoursePractice>(),
                    columns: new[] { "CourseChapterId", "KnowledgePointId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_CoursePractice_CourseChapterId_KnowledgePointId",
                    table: GetShardingTableName<CoursePractice>());
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_CoursePractice_CourseChapterId",
                    table: GetShardingTableName<CoursePractice>(),
                    column: "CourseChapterId",
                    unique: true);
            }

        }
    }
}
