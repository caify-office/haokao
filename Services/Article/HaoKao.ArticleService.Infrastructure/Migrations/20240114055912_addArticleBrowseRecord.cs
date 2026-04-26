using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ArticleService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ArticleService.Infrastructure.Migrations
{
    public partial class addArticleBrowseRecord : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ArticleBrowseRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ArticleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ArticleBrowseRecord", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ArticleBrowseRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ArticleBrowseRecord>());
            }

        }
    }
}
