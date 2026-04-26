using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseService.Domain.CourseMaterialsModule;
using HaoKao.CourseService.Domain.CoursePracticeModule;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseService.Infrastructure.Migrations
{
    public partial class addSmartAssistanceAttribute : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "KnowledgePointId",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci",
                    oldClrType: typeof(string),
                    oldType: "varchar(2000)",
                    oldMaxLength: 2000,
                    oldNullable: true,
                    oldComment: "知识点")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "KnowledgePointIds",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(2000)",
                    maxLength: 2000,
                    nullable: true,
                    comment: "知识点")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "关联的知识点名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "QuestionCategoryId",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "char(36)",
                    nullable: true,
                    collation: "ascii_general_ci",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)")
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "ChapterNodeId",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "char(36)",
                    nullable: true,
                    collation: "ascii_general_ci",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)")
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "KnowledgePointId",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "关联的知识点名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
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


            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "KnowledgePointId",
                    table: GetShardingTableName<CourseMaterials>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CourseMaterials>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "关联的知识点名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropColumn(
                    name: "KnowledgePointIds",
                    table: GetShardingTableName<CourseVideo>());
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.DropColumn(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CourseVideo>());
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropColumn(
                    name: "KnowledgePointId",
                    table: GetShardingTableName<CoursePractice>());
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropColumn(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CoursePractice>());
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.DropColumn(
                    name: "QuestionIds",
                    table: GetShardingTableName<CoursePractice>());
            }


            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.DropColumn(
                    name: "KnowledgePointId",
                    table: GetShardingTableName<CourseMaterials>());
            }


            if (IsCreateShardingTable<CourseMaterials>())
            {
                migrationBuilder.DropColumn(
                    name: "KnowledgePointName",
                    table: GetShardingTableName<CourseMaterials>());
            }


            if (IsCreateShardingTable<CourseVideo>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "KnowledgePointId",
                    table: GetShardingTableName<CourseVideo>(),
                    type: "varchar(2000)",
                    maxLength: 2000,
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    comment: "知识点",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "QuestionCategoryId",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)",
                    oldNullable: true)
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }


            if (IsCreateShardingTable<CoursePractice>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "ChapterNodeId",
                    table: GetShardingTableName<CoursePractice>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)",
                    oldNullable: true)
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }

        }
    }
}
