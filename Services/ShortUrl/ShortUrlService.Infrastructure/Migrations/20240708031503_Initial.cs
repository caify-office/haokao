using System;
using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ShortUrlService.Domain.Entities;

namespace ShortUrlService.Infrastructure.Migrations
{
    public partial class Initial : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<RegisterApp>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<RegisterApp>(),
                    columns: table => new
                    {
                        Id = table.Column<long>(type: "bigint", nullable: false)
                            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                        AppName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "应用名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AppCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "应用编码")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AppSecret = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false, comment: "应用密钥")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, defaultValue: "", comment: "应用描述")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsEnable = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true, comment: "是否启用"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        AppDomains = table.Column<string>(type: "json", nullable: false, comment: "应用域名")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_RegisterApp", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<ShortUrl>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ShortUrl>(),
                    columns: table => new
                    {
                        Id = table.Column<long>(type: "bigint", nullable: false)
                            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                        RegisterAppId = table.Column<long>(type: "bigint", nullable: false),
                        ShortKey = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "短链接后缀")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        OriginUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "原始Url")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AccessLimit = table.Column<int>(type: "int", nullable: false, comment: "可访问次数"),
                        ExpiredTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "过期时间"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        IsDelete = table.Column<ulong>(type: "bit", nullable: false, defaultValue: 0ul, comment: "是否删除标识")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ShortUrl", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<ShortUrl>("FK_ShortUrl_RegisterApp_RegisterAppId"),
                            column: x => x.RegisterAppId,
                            principalTable: GetShardingTableName<RegisterApp>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<AccessLog>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<AccessLog>(),
                    columns: table => new
                    {
                        Id = table.Column<long>(type: "bigint", nullable: false),
                        ShortUrlId = table.Column<long>(type: "bigint", nullable: false, comment: "短链接Id"),
                        Ip = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "IP地址")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        OsType = table.Column<int>(type: "int", nullable: false, comment: "系统类型"),
                        BrowserType = table.Column<int>(type: "int", nullable: false, comment: "浏览器类型"),
                        UserAgent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "UserAgent")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "访问时间")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_AccessLog", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<AccessLog>("FK_AccessLog_ShortUrl_ShortUrlId"),
                            column: x => x.ShortUrlId,
                            principalTable: GetShardingTableName<ShortUrl>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<AccessLog>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AccessLog_ShortUrlId",
                    table: GetShardingTableName<AccessLog>(),
                    column: "ShortUrlId");
            }


            if (IsCreateShardingTable<RegisterApp>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_RegisterApp_AppCode_AppSecret",
                    table: GetShardingTableName<RegisterApp>(),
                    columns: new[] { "AppCode", "AppSecret" },
                    unique: true);
            }


            if (IsCreateShardingTable<RegisterApp>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_RegisterApp_AppName_AppCode",
                    table: GetShardingTableName<RegisterApp>(),
                    columns: new[] { "AppName", "AppCode" },
                    unique: true);
            }


            if (IsCreateShardingTable<ShortUrl>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ShortUrl_RegisterAppId_OriginUrl_IsDelete",
                    table: GetShardingTableName<ShortUrl>(),
                    columns: new[] { "RegisterAppId", "OriginUrl", "IsDelete" });
            }


            if (IsCreateShardingTable<ShortUrl>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ShortUrl_ShortKey_IsDelete",
                    table: GetShardingTableName<ShortUrl>(),
                    columns: new[] { "ShortKey", "IsDelete" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AccessLog>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<AccessLog>());
            }


            if (IsCreateShardingTable<ShortUrl>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ShortUrl>());
            }


            if (IsCreateShardingTable<RegisterApp>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<RegisterApp>());
            }

        }
    }
}
