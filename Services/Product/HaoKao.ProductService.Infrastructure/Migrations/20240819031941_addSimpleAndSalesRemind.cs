using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addSimpleAndSalesRemind : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SalesRemind",
                table: "ProductPackage",
                type: "text",
                nullable: true,
                comment: "售后提醒")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SimpleName",
                table: "ProductPackage",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "简称")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesRemind",
                table: "ProductPackage");

            migrationBuilder.DropColumn(
                name: "SimpleName",
                table: "ProductPackage");
        }
    }
}
