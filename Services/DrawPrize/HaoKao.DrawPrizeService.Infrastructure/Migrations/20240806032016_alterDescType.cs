using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DrawPrizeService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DrawPrizeService.Infrastructure.Migrations
{
    public partial class alterDescType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Desc",
                    table: GetShardingTableName<DrawPrize>(),
                    type: "MEDIUMTEXT",
                    nullable: true,
                    comment: "说明",
                    oldClrType: typeof(string),
                    oldType: "varchar(2000)",
                    oldMaxLength: 2000,
                    oldNullable: true,
                    oldComment: "说明")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Desc",
                    table: GetShardingTableName<DrawPrize>(),
                    type: "varchar(2000)",
                    maxLength: 2000,
                    nullable: true,
                    comment: "说明",
                    oldClrType: typeof(string),
                    oldType: "MEDIUMTEXT",
                    oldNullable: true,
                    oldComment: "说明")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
