using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.NoticeService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.NoticeService.Infrastructure.Migrations
{
    public partial class alterCreatorName : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Notice>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<Notice>(),
                    type: "varchar(40)",
                    nullable: true,
                    comment: "创建人名称",
                    oldClrType: typeof(string),
                    oldType: "nvarchar(40)",
                    oldNullable: true,
                    oldComment: "创建人名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Notice>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<Notice>(),
                    type: "nvarchar(40)",
                    nullable: true,
                    comment: "创建人名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(40)",
                    oldNullable: true,
                    oldComment: "创建人名称")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
