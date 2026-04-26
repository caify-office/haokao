using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.BurialPointService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.BurialPointService.Infrastructure.Migrations
{
    public partial class addInitdata : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<BurialPoint>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<BurialPoint>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        BelongingPortType = table.Column<int>(type: "int", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_BurialPoint", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<BrowseRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<BrowseRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        BurialPointId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "用户昵称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        BrowseData = table.Column<string>(type: "json", nullable: true, comment: "浏览信息")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsPaidUser = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_BrowseRecord", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<BrowseRecord>("FK_BrowseRecord_BurialPoint_BurialPointId"),
                            column: x => x.BurialPointId,
                            principalTable: GetShardingTableName<BurialPoint>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<BrowseRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_BrowseRecord_BurialPointId",
                    table: GetShardingTableName<BrowseRecord>(),
                    column: "BurialPointId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<BrowseRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<BrowseRecord>());
            }


            if (IsCreateShardingTable<BurialPoint>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<BurialPoint>());
            }

        }
    }
}
