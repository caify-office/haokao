using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LiveBroadcastService.Infrastructure.Migrations
{
    public partial class alterPullUrlType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LiveVideo>())
            {
                var liveAddress = @"{""RTS"": ""artc://live.haokao123.com/AppNewHaoKao/string?auth_key=63850581385-0-0-8ebb544367c74bdd9e2e27ee6db0da15"", ""RTMP"": ""rtmp://live.haokao123.com/AppNewHaoKao/string?auth_key=63850581385-0-0-8ebb544367c74bdd9e2e27ee6db0da15""}";
                migrationBuilder.Sql($"Update {GetShardingTableName<LiveVideo>()} Set LiveAddress ='{liveAddress}' ");
                migrationBuilder.AlterColumn<string>(
                    name: "LiveAddress",
                    table: GetShardingTableName<LiveVideo>(),
                    type: "json",
                    nullable: true,
                    comment: "播流地址",
                    oldClrType: typeof(string),
                    oldType: "varchar(300)",
                    oldMaxLength: 300,
                    oldNullable: true,
                    oldComment: "播流地址")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LiveVideo>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "LiveAddress",
                    table: GetShardingTableName<LiveVideo>(),
                    type: "varchar(300)",
                    maxLength: 300,
                    nullable: true,
                    comment: "播流地址",
                    oldClrType: typeof(string),
                    oldType: "json",
                    oldNullable: true,
                    oldComment: "播流地址")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
