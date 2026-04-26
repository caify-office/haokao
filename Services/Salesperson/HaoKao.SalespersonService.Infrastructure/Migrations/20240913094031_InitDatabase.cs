using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.SalespersonService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.SalespersonService.Infrastructure.Migrations
{
    public partial class InitDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<EnterpriseWeChatConfig>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<EnterpriseWeChatConfig>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CorpId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "企业Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CorpName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "企业名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CorpSecret = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "应用的凭证密钥")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_EnterpriseWeChatConfig", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<EnterpriseWeChatContact>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<EnterpriseWeChatContact>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        FollowUserId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "添加了此外部联系人的企业成员Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        FollowUserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "添加了此外部联系人的企业成员名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Type = table.Column<int>(type: "int", nullable: false, comment: "联系人的类型，1表示该外部联系人是微信用户，2表示该外部联系人是企业微信用户"),
                        Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "用户名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        UserId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "微信用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        UnionId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "微信unionid")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_EnterpriseWeChatContact", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Salesperson>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Salesperson>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        RealName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "真实姓名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "手机号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        EnterpriseWeChatUserId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "企业微信用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        EnterpriseWeChatUserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "企业微信昵称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        EnterpriseWeChatConfigId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "企业微信配置Id", collation: "ascii_general_ci"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Salesperson", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<Salesperson>("FK_Salesperson_EnterpriseWeChatConfig_EnterpriseWeChatConfigId"),
                            column: x => x.EnterpriseWeChatConfigId,
                            principalTable: GetShardingTableName<EnterpriseWeChatConfig>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<EnterpriseWeChatConfig>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_EnterpriseWeChatConfig_TenantId",
                    table: GetShardingTableName<EnterpriseWeChatConfig>(),
                    column: "TenantId");
            }


            if (IsCreateShardingTable<EnterpriseWeChatContact>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_EnterpriseWeChatContact_FollowUserId",
                    table: GetShardingTableName<EnterpriseWeChatContact>(),
                    column: "FollowUserId");
            }


            if (IsCreateShardingTable<EnterpriseWeChatContact>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_EnterpriseWeChatContact_UnionId",
                    table: GetShardingTableName<EnterpriseWeChatContact>(),
                    column: "UnionId");
            }


            if (IsCreateShardingTable<Salesperson>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Salesperson_EnterpriseWeChatConfigId",
                    table: GetShardingTableName<Salesperson>(),
                    column: "EnterpriseWeChatConfigId");
            }


            if (IsCreateShardingTable<Salesperson>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Salesperson_Phone_RealName_TenantId",
                    table: GetShardingTableName<Salesperson>(),
                    columns: new[] { "Phone", "RealName", "TenantId" });
            }


            if (IsCreateShardingTable<Salesperson>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Salesperson_RealName_TenantId",
                    table: GetShardingTableName<Salesperson>(),
                    columns: new[] { "RealName", "TenantId" });
            }


            if (IsCreateShardingTable<Salesperson>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Salesperson_TenantId",
                    table: GetShardingTableName<Salesperson>(),
                    column: "TenantId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<EnterpriseWeChatContact>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<EnterpriseWeChatContact>());
            }


            if (IsCreateShardingTable<Salesperson>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Salesperson>());
            }


            if (IsCreateShardingTable<EnterpriseWeChatConfig>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<EnterpriseWeChatConfig>());
            }

        }
    }
}
