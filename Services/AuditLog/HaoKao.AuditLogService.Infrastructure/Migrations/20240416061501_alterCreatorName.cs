using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.AuditLogService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.AuditLogService.Infrastructure.Migrations
{
    public partial class alterCreatorName : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AuditLog>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<AuditLog>(),
                    type: "varchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(40)",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AuditLog>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<AuditLog>(),
                    type: "nvarchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "varchar(40)",
                    oldNullable: true)
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
