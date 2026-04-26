using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DrawPrizeService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DrawPrizeService.Infrastructure.Migrations
{
    public partial class deleteDrawPrizeUrl : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.DropColumn(
                    name: "DrawPrizeUrl",
                    table: GetShardingTableName<DrawPrize>());
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "DrawPrizeUrl",
                    table: GetShardingTableName<DrawPrize>(),
                    type: "varchar(1000)",
                    maxLength: 1000,
                    nullable: true,
                    comment: "抽奖链接")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
