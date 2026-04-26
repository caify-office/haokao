using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class moveAnswering : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answering",
                table: "ProductPackage");

            migrationBuilder.AddColumn<bool>(
                name: "Answering",
                table: "Product",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answering",
                table: "Product");

            migrationBuilder.AddColumn<bool>(
                name: "Answering",
                table: "ProductPackage",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
