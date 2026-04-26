using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.WebsiteConfigurationService.Infrastructure.Migrations
{
    public partial class addIndex : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<WebsiteTemplate>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_WebsiteTemplate_ColumnId_TenantId",
                    table: GetShardingTableName<WebsiteTemplate>(),
                    columns: new[] { "ColumnId", "TenantId" });
            }


            if (IsCreateShardingTable<TemplateStyle>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_TemplateStyle_DomainName_TenantId",
                    table: GetShardingTableName<TemplateStyle>(),
                    columns: new[] { "DomainName", "TenantId" });
            }


            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Column_EnglishName_DomainName_TenantId",
                    table: GetShardingTableName<Column>(),
                    columns: new[] { "EnglishName", "DomainName", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<WebsiteTemplate>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_WebsiteTemplate_ColumnId_TenantId",
                    table: GetShardingTableName<WebsiteTemplate>());
            }


            if (IsCreateShardingTable<TemplateStyle>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_TemplateStyle_DomainName_TenantId",
                    table: GetShardingTableName<TemplateStyle>());
            }


            if (IsCreateShardingTable<Column>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_Column_EnglishName_DomainName_TenantId",
                    table: GetShardingTableName<Column>());
            }

        }
    }
}
