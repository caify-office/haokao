using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ArticleService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ArticleService.Infrastructure.Migrations
{
    public partial class addIsExternalURL : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Article>())
            {
                migrationBuilder.AddColumn<bool>(
                    name: "IsExternalURL",
                    table: GetShardingTableName<Article>(),
                    type: "tinyint(1)",
                    nullable: false,
                    defaultValue: false,
                    comment: "是否外部链接");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Article>())
            {
                migrationBuilder.DropColumn(
                    name: "IsExternalURL",
                    table: GetShardingTableName<Article>());
            }

        }
    }
}
