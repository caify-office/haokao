using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addIsExperience : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExperience",
                table: "ProductPackage",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                comment: "是否体验产品包");

            migrationBuilder.AddColumn<bool>(
                name: "IsExperience",
                table: "Product",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                comment: "是否体验产品");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExperience",
                table: "ProductPackage");

            migrationBuilder.DropColumn(
                name: "IsExperience",
                table: "Product");
        }
    }
}
