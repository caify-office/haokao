using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class addProductEnable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_ProductType_Year_IsShelves_TenantId",
                table: "Product");

            migrationBuilder.AddColumn<bool>(
                name: "Enable",
                table: "Product",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductType_Year_Enable_IsShelves_TenantId",
                table: "Product",
                columns: new[] { "ProductType", "Year", "Enable", "IsShelves", "TenantId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_ProductType_Year_Enable_IsShelves_TenantId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Enable",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductType_Year_IsShelves_TenantId",
                table: "Product",
                columns: new[] { "ProductType", "Year", "IsShelves", "TenantId" });
        }
    }
}
