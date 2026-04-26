using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseMaterialsModule;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class alterCourseMaterialsVideoIndex : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_CourseVideo_CourseChapterId_TenantId",
                    table: GetShardingTableName<CourseVideo>());
            }


            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_CourseMaterials_CourseChapterId_TenantId",
                    table: GetShardingTableName<CourseMaterials>());
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_CourseVideo_CourseChapterId_KnowledgePointId_TenantId",
                    table: GetShardingTableName<CourseVideo>(),
                    columns: new[] { "CourseChapterId", "KnowledgePointId", "TenantId" });
            }


            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_CourseMaterials_CourseChapterId_KnowledgePointId_TenantId",
                    table: GetShardingTableName<CourseMaterials>(),
                    columns: new[] { "CourseChapterId", "KnowledgePointId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_CourseVideo_CourseChapterId_KnowledgePointId_TenantId",
                    table: GetShardingTableName<CourseVideo>());
            }


            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_CourseMaterials_CourseChapterId_KnowledgePointId_TenantId",
                    table: GetShardingTableName<CourseMaterials>());
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_CourseVideo_CourseChapterId_TenantId",
                    table: GetShardingTableName<CourseVideo>(),
                    columns: new[] { "CourseChapterId", "TenantId" });
            }


            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_CourseMaterials_CourseChapterId_TenantId",
                    table: GetShardingTableName<CourseMaterials>(),
                    columns: new[] { "CourseChapterId", "TenantId" });
            }

        }
    }
}
