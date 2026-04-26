using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ArticleService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ArticleService.Infrastructure.Migrations
{
    public partial class addPreviewUrl : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Article>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "PreviewUrl",
                    table: GetShardingTableName<Article>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "预览图")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Article>())
            {
                migrationBuilder.DropColumn(
                    name: "PreviewUrl",
                    table: GetShardingTableName<Article>());
            }

        }
    }
}
