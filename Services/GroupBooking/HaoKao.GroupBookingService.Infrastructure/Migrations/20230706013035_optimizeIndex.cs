using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.GroupBookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.GroupBookingService.Infrastructure.Migrations
{
    public partial class optimizeIndex : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_GroupSituation_DataName_SuitableSubjects_TenantId",
                    table: GetShardingTableName<GroupSituation>());
            }


            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_GroupSituation_GroupDataId_TenantId",
                    table: GetShardingTableName<GroupSituation>());
            }


            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_GroupData_DataName_SuitableSubjects_TenantId",
                    table: GetShardingTableName<GroupData>());
            }


            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_GroupData_State_TenantId",
                    table: GetShardingTableName<GroupData>());
            }


            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "SuitableSubjects",
                    table: GetShardingTableName<GroupSituation>(),
                    type: "text",
                    nullable: true,
                    comment: "适用科目",
                    oldClrType: typeof(string),
                    oldType: "varchar(500)",
                    oldMaxLength: 500,
                    oldNullable: true,
                    oldComment: "适用科目")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "SuitableSubjects",
                    table: GetShardingTableName<GroupData>(),
                    type: "text",
                    nullable: true,
                    comment: "适用科目",
                    oldClrType: typeof(string),
                    oldType: "varchar(500)",
                    oldMaxLength: 500,
                    oldNullable: true,
                    oldComment: "适用科目")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GroupSituation_GroupDataId_SuccessTime_DataName_TenantId",
                    table: GetShardingTableName<GroupSituation>(),
                    columns: new[] { "GroupDataId", "SuccessTime", "DataName", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_GroupSituation_GroupDataId_SuccessTime_DataName_TenantId",
                    table: GetShardingTableName<GroupSituation>());
            }


            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "SuitableSubjects",
                    table: GetShardingTableName<GroupSituation>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "适用科目",
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldNullable: true,
                    oldComment: "适用科目")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "SuitableSubjects",
                    table: GetShardingTableName<GroupData>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "适用科目",
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldNullable: true,
                    oldComment: "适用科目")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GroupSituation_DataName_SuitableSubjects_TenantId",
                    table: GetShardingTableName<GroupSituation>(),
                    columns: new[] { "DataName", "SuitableSubjects", "TenantId" });
            }


            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GroupSituation_GroupDataId_TenantId",
                    table: GetShardingTableName<GroupSituation>(),
                    columns: new[] { "GroupDataId", "TenantId" });
            }


            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GroupData_DataName_SuitableSubjects_TenantId",
                    table: GetShardingTableName<GroupData>(),
                    columns: new[] { "DataName", "SuitableSubjects", "TenantId" });
            }


            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GroupData_State_TenantId",
                    table: GetShardingTableName<GroupData>(),
                    columns: new[] { "State", "TenantId" });
            }

        }
    }
}
