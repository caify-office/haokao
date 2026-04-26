using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudentService.Infrastructure.Migrations
{
    public partial class InitStudentAllocationConfig : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<StudentAllocationConfig>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<StudentAllocationConfig>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Data = table.Column<string>(type: "json", nullable: false, comment: "配置数据")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        WaysOfAllocation = table.Column<int>(type: "int", nullable: false, comment: "分配方式"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_StudentAllocationConfig", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<StudentAllocationConfig>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_StudentAllocationConfig_TenantId",
                    table: GetShardingTableName<StudentAllocationConfig>(),
                    column: "TenantId",
                    unique: true);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<StudentAllocationConfig>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<StudentAllocationConfig>());
            }

        }
    }
}
