using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.TenantService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.TenantService.Infrastructure.Migrations
{
    public partial class initTenantServiceDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Tenant>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        TenantName = table.Column<string>(type: "varchar(500)", nullable: true, comment: "租户名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantNo = table.Column<string>(type: "varchar(20)", nullable: true, comment: "租户编号或代码")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SystemModule = table.Column<long>(type: "bigint", nullable: false),
                        ReleaseState = table.Column<int>(type: "int", nullable: false, comment: "发布状态"),
                        StartState = table.Column<int>(type: "int", nullable: false, comment: "启用禁用状态"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        Instructions = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "说明")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AdminUserAcount = table.Column<string>(type: "varchar(36)", nullable: true, comment: "租户管理员账号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AdminUserName = table.Column<string>(type: "varchar(36)", nullable: true, comment: "租户管理员名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AdminPhone = table.Column<string>(type: "varchar(36)", nullable: true, comment: "租户管理员手机号码")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Tenant", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

                migrationBuilder.DropTable(
                    name: GetShardingTableName<Tenant>());
      

        }
    }
}
