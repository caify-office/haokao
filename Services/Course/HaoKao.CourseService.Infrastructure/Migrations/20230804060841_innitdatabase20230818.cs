using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class innitdatabase20230818 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "VideoName",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "视频名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "视频名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "SourceName",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "视频源名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "视频源名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "Duration",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "decimal(18,2)",
                    precision: 18,
                    scale: 2,
                    nullable: false,
                    comment: "及格分数",
                    oldClrType: typeof(decimal),
                    oldType: "decimal(65,30)");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "VideoName",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "视频名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(500)",
                    oldMaxLength: 500,
                    oldNullable: true,
                    oldComment: "视频名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "SourceName",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "视频源名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(500)",
                    oldMaxLength: 500,
                    oldNullable: true,
                    oldComment: "视频源名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<decimal>(
                    name: "Duration",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "decimal(65,30)",
                    nullable: false,
                    oldClrType: typeof(decimal),
                    oldType: "decimal(18,2)",
                    oldPrecision: 18,
                    oldScale: 2,
                    oldComment: "及格分数");
            }

        }
    }
}
