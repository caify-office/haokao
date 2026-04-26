using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class add_displayName_to_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GiveAwayAList",
                table: "Product",
                type: "json",
                nullable: true,
                comment: "赠送列表",
                oldClrType: typeof(string),
                oldType: "varchar(800)",
                oldMaxLength: 800,
                oldNullable: true,
                oldComment: "赠送列表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Product",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "显示名称")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "GiveAwayAList",
                table: "Product",
                type: "varchar(800)",
                maxLength: 800,
                nullable: true,
                comment: "赠送列表",
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true,
                oldComment: "赠送列表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
