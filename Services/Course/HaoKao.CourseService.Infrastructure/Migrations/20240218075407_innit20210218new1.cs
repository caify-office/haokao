using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseChapterModule;
using HaoKao.CourseService.Domain.CourseMaterialsModule;
using HaoKao.CourseService.Domain.CourseModule;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class innit20210218new1 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.DropColumn(
                    name: "StudyMatrialId",
                    table: GetShardingTableName<Course>());
            }


            if (IsCreateShardingTable<CourseChapter>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "Sort",
                    table: GetShardingTableName<CourseChapter>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
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


            if (IsCreateShardingTable<CourseChapter>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_CourseChapter_CourseId_TenantId",
                    table: GetShardingTableName<CourseChapter>(),
                    columns: new[] { "CourseId", "TenantId" });
            }


            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Course_Id_Name_SubjectId_State_TenantId",
                    table: GetShardingTableName<Course>(),
                    columns: new[] { "Id", "Name", "SubjectId", "State", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
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


            if (IsCreateShardingTable<CourseChapter>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_CourseChapter_CourseId_TenantId",
                    table: GetShardingTableName<CourseChapter>());
            }


            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Course_Id_Name_SubjectId_State_TenantId",
                    table: GetShardingTableName<Course>());
            }


            if (IsCreateShardingTable<CourseChapter>())
            {
                migrationBuilder.DropColumn(
                    name: "Sort",
                    table: GetShardingTableName<CourseChapter>());
            }


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
    }
}
