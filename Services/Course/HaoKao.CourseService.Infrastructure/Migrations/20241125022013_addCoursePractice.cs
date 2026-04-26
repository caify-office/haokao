using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CoursePracticeModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class addCoursePractice : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<CoursePractice>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CourseChapterId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CourseChapterName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "课程章节名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ChapterNodeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ChapterNodeName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "试题章节名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        QuestionCategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        QuestionCategoryName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "试题分类名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_CoursePractice", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<CoursePractice>());
            }

        }
    }
}
