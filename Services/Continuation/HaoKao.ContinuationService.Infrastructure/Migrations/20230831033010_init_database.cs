using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ContinuationService.Domain.ContinuationAuditModule;
using HaoKao.ContinuationService.Domain.ContinuationSetupModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ContinuationService.Infrastructure.Migrations
{
    public partial class init_database : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<ContinuationAudit>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ContinuationAudit>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SetupId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "续读配置Id", collation: "ascii_general_ci"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "产品Id", collation: "ascii_general_ci"),
                        ProductName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "产品名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AgreementId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "协议Id", collation: "ascii_general_ci"),
                        AgreementName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "协议名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ProductGifts = table.Column<string>(type: "json", nullable: true, comment: "产品的赠品Id集合")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ExpiryTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "产品过期时间"),
                        StudentName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "学员姓名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Reason = table.Column<Guid>(type: "char(36)", nullable: false, comment: "续读原因", collation: "ascii_general_ci"),
                        Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "详细描述")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Evidences = table.Column<string>(type: "json", nullable: false, comment: "相关证明")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AuditState = table.Column<int>(type: "int", nullable: false, comment: "审核状态"),
                        AuditReason = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "不通过原因")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        RestOfCount = table.Column<int>(type: "int", nullable: false, comment: "剩余申请次数"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "申请时间"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "申请人")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ContinuationAudit", x => x.Id);
                    },
                    comment: "续读审核")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ContinuationSetup>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ContinuationSetup>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "续读申请开始时间"),
                        EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "续读申请结束时间"),
                        ExpiryTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "续读后的到期时间"),
                        Products = table.Column<string>(type: "json", nullable: false, comment: "产品集合")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Enable = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否启用"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsDelete = table.Column<ulong>(type: "bit", nullable: false, comment: "是否删除标识")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ContinuationSetup", x => x.Id);
                    },
                    comment: "续读配置")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ContinuationAudit>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ContinuationAudit_ProductId_AgreementId_SetupId_CreatorId_Te~",
                    table: GetShardingTableName<ContinuationAudit>(),
                    columns: new[] { "ProductId", "AgreementId", "SetupId", "CreatorId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ContinuationAudit>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ContinuationAudit>());
            }


            if (IsCreateShardingTable<ContinuationSetup>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ContinuationSetup>());
            }

        }
    }
}