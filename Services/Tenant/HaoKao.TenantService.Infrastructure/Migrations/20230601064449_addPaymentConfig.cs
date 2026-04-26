using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.TenantService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.TenantService.Infrastructure.Migrations
{
    public partial class addPaymentConfig : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
                migrationBuilder.AddColumn<string>(
                    name: "PaymentConfigs",
                    table: GetShardingTableName<Tenant>(),
                    type: "text",
                    nullable: true,
                    comment: "收款配置")
                   .Annotation("MySql:CharSet", "utf8mb4");
            

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
                migrationBuilder.DropColumn(
                    name: "PaymentConfigs",
                    table: GetShardingTableName<Tenant>());
            

        }
    }
}
