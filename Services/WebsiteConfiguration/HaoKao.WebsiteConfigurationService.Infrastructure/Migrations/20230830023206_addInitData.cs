using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.WebsiteConfigurationService.Infrastructure.Migrations
{
    public partial class addInitData : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Column>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        DomainName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "域名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Alias = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "别名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        EnglishName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "英文名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Column", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<Column>("FK_Column_Column_ParentId"),
                            column: x => x.ParentId,
                            principalTable: GetShardingTableName<Column>(),
                            principalColumn: "Id");
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<WebsiteTemplate>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<WebsiteTemplate>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        WebsiteTemplateType = table.Column<long>(type: "bigint", nullable: false),
                        Content = table.Column<string>(type: "mediumtext", nullable: true, comment: "内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Desc = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "描述")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ColumnId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ColumnName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "所属栏目名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_WebsiteTemplate", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Column>(),
                    columns: new[] { "Id", "Alias", "CreateTime", "DomainName", "EnglishName", "Name", "ParentId", "TenantId", "UpdateTime" },
                    values: new object[] { new Guid("08dba600-d166-42d5-8630-2cd288dfc9ea"), "好考网-首页", new DateTime(2023, 8, 30, 10, 32, 6, 487, DateTimeKind.Local).AddTicks(1973), "localhost:8088", "index", "首页", null, "00000000-0000-0000-0000-000000000000", new DateTime(2023, 8, 30, 10, 32, 6, 487, DateTimeKind.Local).AddTicks(2015) });
            }


            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Column_ParentId",
                    table: GetShardingTableName<Column>(),
                    column: "ParentId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Column>());
            }


            if (IsCreateShardingTable<WebsiteTemplate>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<WebsiteTemplate>());
            }

        }
    }
}
