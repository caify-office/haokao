using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LecturerService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LecturerService.Infrastructure.Migrations
{
    public partial class addProdoctPackageids : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "PermissionIds",
                    table: GetShardingTableName<Lecturer>(),
                    type: "json",
                    nullable: true,
                    comment: "关联的产品包下所有的权限id")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "ProductPackageIds",
                    table: GetShardingTableName<Lecturer>(),
                    type: "json",
                    nullable: true,
                    comment: "关联的产品包Id")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.DropColumn(
                    name: "PermissionIds",
                    table: GetShardingTableName<Lecturer>());
            }


            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.DropColumn(
                    name: "ProductPackageIds",
                    table: GetShardingTableName<Lecturer>());
            }

        }
    }
}
