using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ArticleService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ArticleService.Infrastructure.Migrations
{
    public partial class initDataBase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<Article>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Article>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "标题")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Column = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Category = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        IsTopping = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        Sort = table.Column<int>(type: "int", nullable: false),
                        IsDisplayedOnHomepage = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        IsPublish = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        Content = table.Column<string>(type: "mediumtext", nullable: true, comment: "内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Article", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Article>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Article_Column_Category_TenantId",
                    table: GetShardingTableName<Article>(),
                    columns: new[] { "Column", "Category", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Article>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Article>());
            }

        }
    }
}
