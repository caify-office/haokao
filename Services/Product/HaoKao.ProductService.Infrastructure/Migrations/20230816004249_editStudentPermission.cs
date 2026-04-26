using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class editStudentPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "StudentPermission",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SourceMode",
                table: "StudentPermission",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "StudentPermission");

            migrationBuilder.DropColumn(
                name: "SourceMode",
                table: "StudentPermission");
        }
    }
}
