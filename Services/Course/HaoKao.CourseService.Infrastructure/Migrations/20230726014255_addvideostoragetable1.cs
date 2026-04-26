using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.VideoStorageModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class addvideostoragetable1 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<VideoStorage>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<VideoStorage>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        VideoStorageHandlerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        VideoStorageHandlerName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "视频存储器名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ConfigParameter = table.Column<string>(type: "varchar(800)", maxLength: 800, nullable: true, comment: "视频存储器相关的配置")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_VideoStorage", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<VideoStorage>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<VideoStorage>());
            }

        }
    }
}
