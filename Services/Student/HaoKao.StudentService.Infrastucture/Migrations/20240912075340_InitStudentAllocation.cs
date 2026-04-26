using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudentService.Infrastructure.Migrations
{
    public partial class InitStudentAllocation : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Student_RegisterUserId_TenantId",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<StudentAllocation>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<StudentAllocation>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        StudentId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "学员Id", collation: "ascii_general_ci"),
                        RegisterUserId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "注册用户Id", collation: "ascii_general_ci"),
                        UnionId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "微信UnionId")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SalespersonId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "销售人员Id", collation: "ascii_general_ci"),
                        SalespersonName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "销售人员姓名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        EnterpriseWeChatId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "企业微信Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AllocationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "分配时间"),
                        Remark = table.Column<string>(type: "longtext", nullable: true, comment: "备注")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_StudentAllocation", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<StudentAllocation>("FK_StudentAllocation_Student_StudentId"),
                            column: x => x.StudentId,
                            principalTable: GetShardingTableName<Student>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Student_RegisterUserId_TenantId",
                    table: GetShardingTableName<Student>(),
                    columns: new[] { "RegisterUserId", "TenantId" },
                    unique: true);
            }


            if (IsCreateShardingTable<StudentAllocation>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_StudentAllocation_SalespersonName_AllocationTime",
                    table: GetShardingTableName<StudentAllocation>(),
                    columns: new[] { "SalespersonName", "AllocationTime" });
            }


            if (IsCreateShardingTable<StudentAllocation>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_StudentAllocation_StudentId",
                    table: GetShardingTableName<StudentAllocation>(),
                    column: "StudentId",
                    unique: true);
            }


            if (IsCreateShardingTable<StudentAllocation>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_StudentAllocation_StudentId_RegisterUserId_UnionId_TenantId",
                    table: GetShardingTableName<StudentAllocation>(),
                    columns: new[] { "StudentId", "RegisterUserId", "UnionId", "TenantId" },
                    unique: true);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<StudentAllocation>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<StudentAllocation>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Student_RegisterUserId_TenantId",
                    table: GetShardingTableName<Student>());
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Student_RegisterUserId_TenantId",
                    table: GetShardingTableName<Student>(),
                    columns: new[] { "RegisterUserId", "TenantId" });
            }

        }
    }
}
