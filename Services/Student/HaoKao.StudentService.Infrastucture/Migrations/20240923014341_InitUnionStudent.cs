using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudentService.Infrastructure.Migrations
{
    public partial class InitUnionStudent : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UnionStudent>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<UnionStudent>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        RegisterUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        IsPaidStudent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_UnionStudent", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UnionStudent>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UnionStudent_RegisterUserId_TenantId_IsPaidStudent",
                    table: GetShardingTableName<UnionStudent>(),
                    columns: new[] { "RegisterUserId", "TenantId", "IsPaidStudent" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UnionStudent>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<UnionStudent>());
            }

        }
    }
}
