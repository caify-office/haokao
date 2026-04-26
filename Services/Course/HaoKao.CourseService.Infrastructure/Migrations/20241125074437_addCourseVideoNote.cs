using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseVideoNoteModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class addCourseVideoNote : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideoNote>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<CourseVideoNote>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        VideoId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "视频id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TimeNode = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "视频时间节点"),
                        CourseVideoNoteType = table.Column<int>(type: "int", nullable: false),
                        NoteContent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "笔记内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "创建者名称")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_CourseVideoNote", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseVideoNote>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_CourseVideoNote_VideoId_CreatorId",
                    table: GetShardingTableName<CourseVideoNote>(),
                    columns: new[] { "VideoId", "CreatorId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideoNote>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<CourseVideoNote>());
            }

        }
    }
}
