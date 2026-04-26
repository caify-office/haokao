using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class alterCreatorName1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatedProducts_Product_ProductId",
                table: "RelatedProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelatedProducts",
                table: "RelatedProducts");

            migrationBuilder.RenameTable(
                name: "RelatedProducts",
                newName: "RelatedProduct");

            migrationBuilder.RenameIndex(
                name: "IX_RelatedProducts_ProductId",
                table: "RelatedProduct",
                newName: "IX_RelatedProduct_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "RelatedProduct",
                type: "varchar(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "RelatedTargetProducName",
                table: "RelatedProduct",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "关联对象产品名称",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "RelatedProduct",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "产品名称",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorName",
                table: "RelatedProduct",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "创建人名称",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "RelatedProduct",
                type: "varchar(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "RelatedProduct",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelatedProduct",
                table: "RelatedProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedProduct_Product_ProductId",
                table: "RelatedProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatedProduct_Product_ProductId",
                table: "RelatedProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RelatedProduct",
                table: "RelatedProduct");

            migrationBuilder.RenameTable(
                name: "RelatedProduct",
                newName: "RelatedProducts");

            migrationBuilder.RenameIndex(
                name: "IX_RelatedProduct_ProductId",
                table: "RelatedProducts",
                newName: "IX_RelatedProducts_ProductId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "RelatedProducts",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "RelatedTargetProducName",
                table: "RelatedProducts",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "关联对象产品名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "RelatedProducts",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "产品名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorName",
                table: "RelatedProducts",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "创建人名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "RelatedProducts",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "RelatedProducts",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RelatedProducts",
                table: "RelatedProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedProducts_Product_ProductId",
                table: "RelatedProducts",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
