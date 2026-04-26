using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.OrderService.Infrastructure.Migrations
{
    public partial class modifyclientfieldtpe : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "ClientId",
                    table: GetShardingTableName<Order>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "客户端ID",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Order>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "ClientId",
                    table: GetShardingTableName<Order>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "客户端ID")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
