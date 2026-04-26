using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LecturerService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LecturerService.Infrastructure.Migrations
{
    public partial class addSubjectInfo : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "SubjectId",
                    table: GetShardingTableName<Lecturer>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "SubjectName",
                    table: GetShardingTableName<Lecturer>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "科目名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Lecturer_SubjectId_Name_TenantId",
                    table: GetShardingTableName<Lecturer>(),
                    columns: new[] { "SubjectId", "Name", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Lecturer_SubjectId_Name_TenantId",
                    table: GetShardingTableName<Lecturer>());
            }


            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.DropColumn(
                    name: "SubjectId",
                    table: GetShardingTableName<Lecturer>());
            }


            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.DropColumn(
                    name: "SubjectName",
                    table: GetShardingTableName<Lecturer>());
            }

        }
    }
}
