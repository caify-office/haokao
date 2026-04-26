using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.GroupBookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.GroupBookingService.Infrastructure.Migrations
{
    public partial class addIndexOptimize : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    name: "IX_GroupData_State_TenantId",
                    table: GetShardingTableName<GroupData>(),
                    columns: new[] { "State", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_GroupSituation_GroupDataId_TenantId",
                    table: GetShardingTableName<GroupSituation>());
            }


            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_GroupData_State_TenantId",
                    table: GetShardingTableName<GroupData>());
            }

        }
    }
}
