using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.GroupBookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.GroupBookingService.Infrastructure.Migrations
{
    public partial class addBasePeopleNumber : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "BasePeopleNumber",
                    table: GetShardingTableName<GroupData>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0,
                    comment: "基础拼团成功人数");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<GroupData>())
            {
                migrationBuilder.DropColumn(
                    name: "BasePeopleNumber",
                    table: GetShardingTableName<GroupData>());
            }

        }
    }
}
