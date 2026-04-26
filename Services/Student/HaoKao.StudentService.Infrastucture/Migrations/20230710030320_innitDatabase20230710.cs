using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudentService.Infrastructure.Migrations
{
    public partial class innitDatabase20230710 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IsOldStudent",
                    table: GetShardingTableName<Student>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Student>())
            {
                migrationBuilder.DropColumn(
                    name: "IsOldStudent",
                    table: GetShardingTableName<Student>());
            }

        }
    }
}
