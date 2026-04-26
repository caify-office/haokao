using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudentService.Infrastructure.Migrations
{
    public partial class AddRegisterUserIdAndUnionIdToStudentFollow : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<StudentFollow>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "RegisterUserId",
                    table: GetShardingTableName<StudentFollow>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    comment: "用户Id",
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<StudentFollow>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "UnionId",
                    table: GetShardingTableName<StudentFollow>(),
                    type: "varchar(100)",
                    maxLength: 100,
                    nullable: true,
                    comment: "微信UnionId")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<StudentFollow>())
            {
                migrationBuilder.DropColumn(
                    name: "RegisterUserId",
                    table: GetShardingTableName<StudentFollow>());
            }


            if (IsCreateShardingTable<StudentFollow>())
            {
                migrationBuilder.DropColumn(
                    name: "UnionId",
                    table: GetShardingTableName<StudentFollow>());
            }

        }
    }
}
