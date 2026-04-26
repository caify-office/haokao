using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class alterCreateNameType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<Order>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "创建者名称",
                    oldClrType: typeof(string),
                    oldType: "nvarchar(40)",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<Order>(),
                    type: "nvarchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "创建者名称")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
