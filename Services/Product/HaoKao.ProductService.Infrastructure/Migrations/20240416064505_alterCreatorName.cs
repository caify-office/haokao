using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class alterCreatorName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatorName",
                table: "ProductPackage",
                type: "varchar(40)",
                nullable: true,
                comment: "创建人名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorName",
                table: "Product",
                type: "varchar(40)",
                nullable: true,
                comment: "创建人名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatorName",
                table: "ProductPackage",
                type: "nvarchar(40)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldNullable: true,
                oldComment: "创建人名称")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorName",
                table: "Product",
                type: "nvarchar(40)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldNullable: true,
                oldComment: "创建人名称")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
