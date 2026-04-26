using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.FeedBackService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.FeedBackService.Infrastructure.Migrations
{
    public partial class ChangeCreatorNameType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<FeedBackReply>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<FeedBackReply>(),
                    type: "varchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(40)",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<FeedBackReply>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "FileUrl",
                    table: GetShardingTableName<FeedBackReply>(),
                    type: "longtext",
                    nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<FeedBack>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<FeedBack>(),
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
            if (IsCreateShardingTable<FeedBackReply>())
            {
                migrationBuilder.DropColumn(
                    name: "FileUrl",
                    table: GetShardingTableName<FeedBackReply>());
            }


            if (IsCreateShardingTable<FeedBackReply>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<FeedBackReply>(),
                    type: "nvarchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "varchar(40)",
                    oldNullable: true)
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<FeedBack>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<FeedBack>(),
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
