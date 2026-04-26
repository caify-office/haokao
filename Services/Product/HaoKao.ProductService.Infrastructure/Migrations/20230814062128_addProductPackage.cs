using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class addProductPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductPackage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    CardImage = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "产品卡片图片")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DetailImage = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "详细介绍图片")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Selling = table.Column<string>(type: "text", nullable: true, comment: "卖点")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiryTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "到期时间"),
                    PreferentialExpiryTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "优惠到期时间"),
                    Year = table.Column<DateTime>(type: "datetime", nullable: false, comment: "所属年份"),
                    FeaturedService = table.Column<string>(type: "text", nullable: true, comment: "特色服务")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hot = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Answering = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Contrast = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Enable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Shelves = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: true, comment: "对比详细介绍")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductList = table.Column<string>(type: "text", nullable: true, comment: "对应的产品列表")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPackage", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPackage");
        }
    }
}
