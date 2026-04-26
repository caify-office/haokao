using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseChapterModule;
using HaoKao.CourseService.Domain.CourseModule;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class innitDatabase20237025 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Course>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "课程名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TeacherJson = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true, comment: "主讲老师json")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Year = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "年份")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "科目名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        UpdateTimeDesc = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "预计更新时间")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        State = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Course", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseChapter>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<CourseChapter>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ParentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CourseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        IsLeaf = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_CourseChapter", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<CourseVideo>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CourseChapterId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        KnowledgePointId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Suffix = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "后缀")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Duration = table.Column<long>(type: "bigint", nullable: false),
                        IsTry = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        VideoName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "视频名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SourceName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "视频源名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        VideoUrl = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "播放url-冗余")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        VideoId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "视频id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Sort = table.Column<int>(type: "int", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_CourseVideo", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Course>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Course>());
            }


            if (IsCreateShardingTable<CourseChapter>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<CourseChapter>());
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<CourseVideo>());
            }

        }
    }
}
