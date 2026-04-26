using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class addSort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "ProductPackage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sort",
                table: "ProductPackage");
        }
    }
}
