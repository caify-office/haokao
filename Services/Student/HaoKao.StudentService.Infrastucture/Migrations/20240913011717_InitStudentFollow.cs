using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudentService.Infrastructure.Migrations
{
    public partial class InitStudentFollow : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<StudentFollow>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<StudentFollow>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        StudentId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "学员Id", collation: "ascii_general_ci"),
                        SalespersonId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "销售人员Id", collation: "ascii_general_ci"),
                        SalespersonName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "销售人员姓名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        EnterpriseWeChatId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "企业微信Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "添加时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_StudentFollow", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<StudentFollow>("FK_StudentFollow_Student_StudentId"),
                            column: x => x.StudentId,
                            principalTable: GetShardingTableName<Student>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<StudentFollow>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_StudentFollow_SalespersonName_TenantId",
                    table: GetShardingTableName<StudentFollow>(),
                    columns: new[] { "SalespersonName", "TenantId" });
            }


            if (IsCreateShardingTable<StudentFollow>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_StudentFollow_StudentId",
                    table: GetShardingTableName<StudentFollow>(),
                    column: "StudentId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<StudentFollow>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<StudentFollow>());
            }

        }
    }
}
