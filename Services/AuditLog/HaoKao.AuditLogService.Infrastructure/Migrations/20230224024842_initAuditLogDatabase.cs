using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.AuditLogService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.AuditLogService.Infrastructure.Migrations
{
    public partial class initAuditLogDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<AuditLog>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<AuditLog>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ServiceModuleName = table.Column<string>(type: "varchar(36)", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Message = table.Column<string>(type: "varchar(255)", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        MessageContent = table.Column<string>(type: "text", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AddressIp = table.Column<string>(type: "varchar(100)", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SourceType = table.Column<int>(type: "int", nullable: false),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantName = table.Column<string>(type: "varchar(100)", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_AuditLog", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<AuditLog>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AuditLog_CreateTime_AddressIp_CreatorName_Message_TenantId_S~",
                    table: GetShardingTableName<AuditLog>(),
                    columns: new[] { "CreateTime", "AddressIp", "CreatorName", "Message", "TenantId", "SourceType" });
            }


            if (IsCreateShardingTable<AuditLog>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AuditLog_TenantId",
                    table: GetShardingTableName<AuditLog>(),
                    column: "TenantId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AuditLog>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<AuditLog>());
            }

        }
    }
}
