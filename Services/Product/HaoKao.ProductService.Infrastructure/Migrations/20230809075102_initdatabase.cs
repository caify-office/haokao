using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class initdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "产品名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    IsShelves = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "产品描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "产品图标图片")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DetailImage = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "产品详情介绍图片")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiryTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Price = table.Column<double>(type: "double", precision: 6, scale: 2, nullable: false, comment: "价格"),
                    DiscountedPrice = table.Column<double>(type: "double", precision: 6, scale: 2, nullable: false, comment: "优惠价格"),
                    AppleProductId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true, comment: "苹果内购产品ID")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Agreement = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GiveAwayAList = table.Column<string>(type: "varchar(800)", maxLength: 800, nullable: true, comment: "赠送列表")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudentPermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StudentName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "学员昵称(即用户昵称)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StudentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrderNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "对应的订单号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, comment: "产品名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiryTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "到期时间"),
                    Enable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPermission", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductPermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubjectName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, comment: "科目名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermissionName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, comment: "产品权限名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermissionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Category = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, comment: "产品权限属性分类")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPermission_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPermission_ProductId",
                table: "ProductPermission",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPermission");

            migrationBuilder.DropTable(
                name: "StudentPermission");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
