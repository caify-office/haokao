using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudentService.Infrastructure.Migrations
{
    public partial class RefactorStudent : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Student_UserId_Phone_NickName_TenantId",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropColumn(
                    name: "Email",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropColumn(
                    name: "IsFirst",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropColumn(
                    name: "IsOldStudent",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropColumn(
                    name: "NickName",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropColumn(
                    name: "Phone",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropColumn(
                    name: "StudyBase",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropColumn(
                    name: "SubjectId",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropColumn(
                    name: "SubjectName",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropColumn(
                    name: "TenantName",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "UserId",
                    table: GetShardingTableName<Student>(),
                    type: "char(36)",
                    nullable: false,
                    comment: "用户Id",
                    collation: "ascii_general_ci",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)")
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "TenantId",
                    table: GetShardingTableName<Student>(),
                    type: "varchar(36)",
                    nullable: false,
                    comment: "租户Id",
                    oldClrType: typeof(string),
                    oldType: "varchar(36)")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AlterColumn<DateTime>(
                    name: "CreateTime",
                    table: GetShardingTableName<Student>(),
                    type: "datetime",
                    nullable: false,
                    comment: "创建时间",
                    oldClrType: typeof(DateTime),
                    oldType: "datetime");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IsPaidStudent",
                    table: GetShardingTableName<Student>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false,
                    comment: "是否付费学员");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Student_UserId_TenantId",
                    table: GetShardingTableName<Student>(),
                    columns: new[] { "UserId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Student_UserId_TenantId",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropColumn(
                    name: "IsPaidStudent",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "UserId",
                    table: GetShardingTableName<Student>(),
                    type: "char(36)",
                    nullable: false,
                    collation: "ascii_general_ci",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)",
                    oldComment: "用户Id")
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "TenantId",
                    table: GetShardingTableName<Student>(),
                    type: "varchar(36)",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(36)",
                    oldComment: "租户Id")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AlterColumn<DateTime>(
                    name: "CreateTime",
                    table: GetShardingTableName<Student>(),
                    type: "datetime",
                    nullable: false,
                    oldClrType: typeof(DateTime),
                    oldType: "datetime",
                    oldComment: "创建时间");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "Email",
                    table: GetShardingTableName<Student>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "邮箱")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "IsFirst",
                    table: GetShardingTableName<Student>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IsOldStudent",
                    table: GetShardingTableName<Student>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false);
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "NickName",
                    table: GetShardingTableName<Student>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "昵称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "Phone",
                    table: GetShardingTableName<Student>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "手机号码")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "StudyBase",
                    table: GetShardingTableName<Student>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "SubjectId",
                    table: GetShardingTableName<Student>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "SubjectName",
                    table: GetShardingTableName<Student>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "科目名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "TenantName",
                    table: GetShardingTableName<Student>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "租户名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Student_UserId_Phone_NickName_TenantId",
                    table: GetShardingTableName<Student>(),
                    columns: new[] { "UserId", "Phone", "NickName", "TenantId" });
            }

        }
    }
}
