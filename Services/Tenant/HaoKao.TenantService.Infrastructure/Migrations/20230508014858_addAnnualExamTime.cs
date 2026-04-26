using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.TenantService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.TenantService.Infrastructure.Migrations
{
    public partial class addAnnualExamTime : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
                migrationBuilder.AddColumn<DateTime>(
                    name: "AnnualExamTime",
                    table: GetShardingTableName<Tenant>(),
                    type: "datetime",
                    nullable: false,
                    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    comment: "年度考试时间");
            

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
                migrationBuilder.DropColumn(
                    name: "AnnualExamTime",
                    table: GetShardingTableName<Tenant>());
            

        }
    }
}
