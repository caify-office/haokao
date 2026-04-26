using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class addCourseVideoInfo : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AddColumn<long>(
                    name: "CateId",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "bigint",
                    nullable: true);
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "CateName",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(100)",
                    maxLength: 100,
                    nullable: true,
                    comment: "视频分类名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "DisplayName",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "显示名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "Tags",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(100)",
                    maxLength: 100,
                    nullable: true,
                    comment: "视频标签")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AddColumn<DateTime>(
                    name: "UpdateTime",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "datetime",
                    nullable: false,
                    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropColumn(
                    name: "CateId",
                    table: GetShardingTableName<CourseVideo>());
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropColumn(
                    name: "CateName",
                    table: GetShardingTableName<CourseVideo>());
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropColumn(
                    name: "DisplayName",
                    table: GetShardingTableName<CourseVideo>());
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropColumn(
                    name: "Tags",
                    table: GetShardingTableName<CourseVideo>());
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropColumn(
                    name: "UpdateTime",
                    table: GetShardingTableName<CourseVideo>());
            }

        }
    }
}
