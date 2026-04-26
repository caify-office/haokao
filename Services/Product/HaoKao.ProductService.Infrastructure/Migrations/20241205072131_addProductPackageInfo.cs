using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addProductPackageInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductPackageId",
                table: "SupervisorClass",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "ProductPackageName",
                table: "SupervisorClass",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "产品包名称")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPackageId",
                table: "SupervisorClass");

            migrationBuilder.DropColumn(
                name: "ProductPackageName",
                table: "SupervisorClass");
        }
    }
}
