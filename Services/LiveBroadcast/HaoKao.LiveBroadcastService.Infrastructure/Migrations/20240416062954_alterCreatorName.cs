using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LiveBroadcastService.Infrastructure.Migrations
{
    public partial class alterCreatorName : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<MutedUser>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<MutedUser>(),
                    type: "varchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(40)",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveReservation>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<LiveReservation>(),
                    type: "varchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(40)",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveOnlineUser>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<LiveOnlineUser>(),
                    type: "varchar(40)",
                    nullable: true,
                    comment: "用户名称",
                    oldClrType: typeof(string),
                    oldType: "nvarchar(40)",
                    oldNullable: true,
                    oldComment: "用户名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveMessage>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<LiveMessage>(),
                    type: "varchar(40)",
                    nullable: true,
                    comment: "用户名称",
                    oldClrType: typeof(string),
                    oldType: "nvarchar(40)",
                    oldNullable: true,
                    oldComment: "用户名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveComment>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<LiveComment>(),
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
            if (IsCreateShardingTable<MutedUser>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<MutedUser>(),
                    type: "nvarchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "varchar(40)",
                    oldNullable: true)
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveReservation>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<LiveReservation>(),
                    type: "nvarchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "varchar(40)",
                    oldNullable: true)
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveOnlineUser>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<LiveOnlineUser>(),
                    type: "nvarchar(40)",
                    nullable: true,
                    comment: "用户名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(40)",
                    oldNullable: true,
                    oldComment: "用户名称")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveMessage>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<LiveMessage>(),
                    type: "nvarchar(40)",
                    nullable: true,
                    comment: "用户名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(40)",
                    oldNullable: true,
                    oldComment: "用户名称")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveComment>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<LiveComment>(),
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
