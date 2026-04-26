using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DrawPrizeService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DrawPrizeService.Infrastructure.Migrations
{
    public partial class AllowEmpty : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.AlterColumn<double>(
                    name: "Probability",
                    table: GetShardingTableName<DrawPrize>(),
                    type: "double",
                    nullable: true,
                    oldClrType: typeof(double),
                    oldType: "double");
            }


            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "DrawPrizeType",
                    table: GetShardingTableName<DrawPrize>(),
                    type: "int",
                    nullable: true,
                    oldClrType: typeof(int),
                    oldType: "int");
            }


            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "DrawPrizeRange",
                    table: GetShardingTableName<DrawPrize>(),
                    type: "int",
                    nullable: true,
                    oldClrType: typeof(int),
                    oldType: "int");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.AlterColumn<double>(
                    name: "Probability",
                    table: GetShardingTableName<DrawPrize>(),
                    type: "double",
                    nullable: false,
                    defaultValue: 0.0,
                    oldClrType: typeof(double),
                    oldType: "double",
                    oldNullable: true);
            }


            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "DrawPrizeType",
                    table: GetShardingTableName<DrawPrize>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0,
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldNullable: true);
            }


            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.AlterColumn<int>(
                    name: "DrawPrizeRange",
                    table: GetShardingTableName<DrawPrize>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0,
                    oldClrType: typeof(int),
                    oldType: "int",
                    oldNullable: true);
            }

        }
    }
}
