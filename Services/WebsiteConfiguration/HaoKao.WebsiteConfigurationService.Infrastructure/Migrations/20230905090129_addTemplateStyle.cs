using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.WebsiteConfigurationService.Infrastructure.Migrations
{
    public partial class addTemplateStyle : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<TemplateStyle>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<TemplateStyle>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        DomainName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "域名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Path = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_TemplateStyle", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<TemplateStyle>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<TemplateStyle>());
            }

        }
    }
}
