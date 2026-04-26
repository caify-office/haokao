using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addStatisticsIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StudentPermission_ProductId_Enable_TenantId",
                table: "StudentPermission",
                columns: new[] { "ProductId", "Enable", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentPermission_StudentId_TenantId",
                table: "StudentPermission",
                columns: new[] { "StudentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPermission_PermissionId_TenantId",
                table: "ProductPermission",
                columns: new[] { "PermissionId", "TenantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentPermission_ProductId_Enable_TenantId",
                table: "StudentPermission");

            migrationBuilder.DropIndex(
                name: "IX_StudentPermission_StudentId_TenantId",
                table: "StudentPermission");

            migrationBuilder.DropIndex(
                name: "IX_ProductPermission_PermissionId_TenantId",
                table: "ProductPermission");
        }
    }
}
