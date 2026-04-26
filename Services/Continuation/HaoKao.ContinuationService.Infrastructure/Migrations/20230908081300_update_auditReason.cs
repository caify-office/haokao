using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ContinuationService.Domain.ContinuationAuditModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ContinuationService.Infrastructure.Migrations
{
    public partial class update_auditReason : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ContinuationAudit>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "AuditReason",
                    table: GetShardingTableName<ContinuationAudit>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: false,
                    comment: "不通过原因",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldComment: "不通过原因")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ContinuationAudit>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "AuditReason",
                    table: GetShardingTableName<ContinuationAudit>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: false,
                    comment: "不通过原因",
                    oldClrType: typeof(string),
                    oldType: "varchar(500)",
                    oldMaxLength: 500,
                    oldComment: "不通过原因")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}