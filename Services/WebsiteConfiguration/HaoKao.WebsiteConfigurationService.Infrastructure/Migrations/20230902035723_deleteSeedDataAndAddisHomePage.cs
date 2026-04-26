using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.WebsiteConfigurationService.Infrastructure.Migrations
{
    public partial class deleteSeedDataAndAddisHomePage : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Column>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dba600-d166-42d5-8630-2cd288dfc9ea"));
            }


            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IsHomePage",
                    table: GetShardingTableName<Column>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.DropColumn(
                    name: "IsHomePage",
                    table: GetShardingTableName<Column>());
            }


            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Column>(),
                    columns: new[] { "Id", "Alias", "CreateTime", "DomainName", "EnglishName", "Icon", "Name", "ParentId", "TenantId", "UpdateTime" },
                    values: new object[] { new Guid("08dba600-d166-42d5-8630-2cd288dfc9ea"), "好考网-首页", new DateTime(2023, 9, 1, 9, 17, 6, 748, DateTimeKind.Local).AddTicks(5218), "localhost:8088", "index", null, "首页", null, "00000000-0000-0000-0000-000000000000", new DateTime(2023, 9, 1, 9, 17, 6, 748, DateTimeKind.Local).AddTicks(5264) });
            }

        }
    }
}
