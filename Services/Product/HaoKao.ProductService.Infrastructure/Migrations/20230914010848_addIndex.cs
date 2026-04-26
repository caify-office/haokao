using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class addIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StudentPermission_StudentId_ProductId_Enable",
                table: "StudentPermission",
                columns: new[] { "StudentId", "ProductId", "Enable" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPackage_ProductType_Year_Enable_Shelves_TenantId",
                table: "ProductPackage",
                columns: new[] { "ProductType", "Year", "Enable", "Shelves", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductType_IsShelves_TenantId",
                table: "Product",
                columns: new[] { "ProductType", "IsShelves", "TenantId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentPermission_StudentId_ProductId_Enable",
                table: "StudentPermission");

            migrationBuilder.DropIndex(
                name: "IX_ProductPackage_ProductType_Year_Enable_Shelves_TenantId",
                table: "ProductPackage");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductType_IsShelves_TenantId",
                table: "Product");
        }
    }
}
