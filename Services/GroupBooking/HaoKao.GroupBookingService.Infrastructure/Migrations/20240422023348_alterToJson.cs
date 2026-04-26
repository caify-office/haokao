using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.GroupBookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.GroupBookingService.Infrastructure.Migrations
{
    public partial class alterToJson : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.Sql(@$"UPDATE {GetShardingTableName<GroupSituation>()} SET {nameof(GroupSituation.SuitableSubjects)}= JSON_ARRAY(SUBSTRING_INDEX({nameof(GroupSituation.SuitableSubjects)}, ',', 1)) ");
                migrationBuilder.AlterColumn<string>(
                    name: "SuitableSubjects",
                    table: GetShardingTableName<GroupSituation>(),
                    type: "json",
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
                migrationBuilder.Sql(@$"UPDATE {GetShardingTableName<GroupData>()} SET {nameof(GroupData.SuitableSubjects)}= JSON_ARRAY(SUBSTRING_INDEX({nameof(GroupData.SuitableSubjects)}, ',', 1)) ");
                migrationBuilder.AlterColumn<string>(
                    name: "SuitableSubjects",
                    table: GetShardingTableName<GroupData>(),
                    type: "json",
                    nullable: true,
                    comment: "适用科目",
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldNullable: true,
                    oldComment: "适用科目")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<GroupSituation>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "SuitableSubjects",
                    table: GetShardingTableName<GroupSituation>(),
                    type: "text",
                    nullable: true,
                    comment: "适用科目",
                    oldClrType: typeof(string),
                    oldType: "json",
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
                    oldType: "json",
                    oldNullable: true,
                    oldComment: "适用科目")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
