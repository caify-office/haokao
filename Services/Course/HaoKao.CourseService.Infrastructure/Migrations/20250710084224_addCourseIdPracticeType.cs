using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CoursePracticeModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class addCourseIdPracticeType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropColumn(
                    name: "QuestionIds",
                    table: GetShardingTableName<CoursePractice>());
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "CourseId",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "PracticeType",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "QuestionConfig",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "json",
                    nullable: true,
                    comment: "试题配置(智辅学习课程，添加课后练习使用)")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "QuestionCount",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropColumn(
                    name: "CourseId",
                    table: GetShardingTableName<CoursePractice>());
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropColumn(
                    name: "PracticeType",
                    table: GetShardingTableName<CoursePractice>());
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropColumn(
                    name: "QuestionConfig",
                    table: GetShardingTableName<CoursePractice>());
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropColumn(
                    name: "QuestionCount",
                    table: GetShardingTableName<CoursePractice>());
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "QuestionIds",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "json",
                    nullable: true,
                    comment: "试题id集合(智辅学习课程，添加课后练习使用)")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
