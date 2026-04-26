using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudentService.Infrastructure.Migrations
{
    public partial class RenameUserId : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.RenameColumn(
                    name: "UserId",
                    table: GetShardingTableName<Student>(),
                    newName: "RegisterUserId");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.RenameIndex(
                    name: "IX_Student_UserId_TenantId",
                    table: GetShardingTableName<Student>(),
                    newName: "IX_Student_RegisterUserId_TenantId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.RenameColumn(
                    name: "RegisterUserId",
                    table: GetShardingTableName<Student>(),
                    newName: "UserId");
            }


            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.RenameIndex(
                    name: "IX_Student_RegisterUserId_TenantId",
                    table: GetShardingTableName<Student>(),
                    newName: "IX_Student_UserId_TenantId");
            }

        }
    }
}
