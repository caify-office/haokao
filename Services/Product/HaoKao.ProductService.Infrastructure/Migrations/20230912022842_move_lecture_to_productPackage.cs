using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class move_lecture_to_productPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LecturerList",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "Selling",
                table: "ProductPackage",
                type: "json",
                nullable: true,
                comment: "卖点",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "卖点")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProductList",
                table: "ProductPackage",
                type: "json",
                nullable: true,
                comment: "对应的产品列表",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "对应的产品列表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "FeaturedService",
                table: "ProductPackage",
                type: "json",
                nullable: true,
                comment: "特色服务",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "特色服务")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Detail",
                table: "ProductPackage",
                type: "json",
                nullable: true,
                comment: "对比详细介绍",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "对比详细介绍")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LecturerList",
                table: "ProductPackage",
                type: "json",
                nullable: true,
                comment: "讲师列表")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LecturerList",
                table: "ProductPackage");

            migrationBuilder.AlterColumn<string>(
                name: "Selling",
                table: "ProductPackage",
                type: "text",
                nullable: true,
                comment: "卖点",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true,
                oldComment: "卖点")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProductList",
                table: "ProductPackage",
                type: "text",
                nullable: true,
                comment: "对应的产品列表",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true,
                oldComment: "对应的产品列表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "FeaturedService",
                table: "ProductPackage",
                type: "text",
                nullable: true,
                comment: "特色服务",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true,
                oldComment: "特色服务")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Detail",
                table: "ProductPackage",
                type: "text",
                nullable: true,
                comment: "对比详细介绍",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true,
                oldComment: "对比详细介绍")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LecturerList",
                table: "Product",
                type: "varchar(800)",
                maxLength: 800,
                nullable: true,
                comment: "讲师列表")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
