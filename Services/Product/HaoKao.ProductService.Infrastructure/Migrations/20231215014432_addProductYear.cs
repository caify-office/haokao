using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class addProductYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_ProductType_IsShelves_TenantId",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "Product",
                type: "varchar(4)",
                maxLength: 4,
                nullable: true,
                comment: "年份")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductType_Year_IsShelves_TenantId",
                table: "Product",
                columns: new[] { "ProductType", "Year", "IsShelves", "TenantId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_ProductType_Year_IsShelves_TenantId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductType_IsShelves_TenantId",
                table: "Product",
                columns: new[] { "ProductType", "IsShelves", "TenantId" });
        }
    }
}
