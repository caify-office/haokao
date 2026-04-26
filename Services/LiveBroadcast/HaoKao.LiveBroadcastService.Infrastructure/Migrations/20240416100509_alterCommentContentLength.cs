using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LiveBroadcastService.Infrastructure.Migrations
{
    public partial class alterCommentContentLength : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LiveComment>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Content",
                    table: GetShardingTableName<LiveComment>(),
                    type: "varchar(300)",
                    maxLength: 300,
                    nullable: true,
                    comment: "评价内容",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "评价内容")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LiveComment>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Content",
                    table: GetShardingTableName<LiveComment>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "评价内容",
                    oldClrType: typeof(string),
                    oldType: "varchar(300)",
                    oldMaxLength: 300,
                    oldNullable: true,
                    oldComment: "评价内容")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
