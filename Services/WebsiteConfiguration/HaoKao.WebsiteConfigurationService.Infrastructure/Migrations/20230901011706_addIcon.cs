using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.WebsiteConfigurationService.Infrastructure.Migrations
{
    public partial class addIcon : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "Icon",
                    table: GetShardingTableName<Column>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "图标")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<Column>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dba600-d166-42d5-8630-2cd288dfc9ea"),
                    columns: new[] { "CreateTime", "UpdateTime" },
                    values: new object[] { new DateTime(2023, 9, 1, 9, 17, 6, 748, DateTimeKind.Local).AddTicks(5218), new DateTime(2023, 9, 1, 9, 17, 6, 748, DateTimeKind.Local).AddTicks(5264) });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.DropColumn(
                    name: "Icon",
                    table: GetShardingTableName<Column>());
            }


            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<Column>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dba600-d166-42d5-8630-2cd288dfc9ea"),
                    columns: new[] { "CreateTime", "UpdateTime" },
                    values: new object[] { new DateTime(2023, 8, 30, 10, 32, 6, 487, DateTimeKind.Local).AddTicks(1973), new DateTime(2023, 8, 30, 10, 32, 6, 487, DateTimeKind.Local).AddTicks(2015) });
            }

        }
    }
}
