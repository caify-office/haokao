using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.TenantService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.TenantService.Infrastructure.Migrations
{
    public partial class addtenantotherfields : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

                migrationBuilder.AddColumn<Guid>(
                    name: "OtherId",
                    table: GetShardingTableName<Tenant>(),
                    type: "char(36)",
                    nullable: true,
                    collation: "ascii_general_ci");
            


           
                migrationBuilder.AddColumn<string>(
                    name: "OtherName",
                    table: GetShardingTableName<Tenant>(),
                    type: "varchar(500)",
                    nullable: true,
                    comment: "其它名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
                migrationBuilder.DropColumn(
                    name: "OtherId",
                    table: GetShardingTableName<Tenant>());
            


            
                migrationBuilder.DropColumn(
                    name: "OtherName",
                    table: GetShardingTableName<Tenant>());
            

        }
    }
}
