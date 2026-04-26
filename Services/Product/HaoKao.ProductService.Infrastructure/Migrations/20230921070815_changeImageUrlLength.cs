using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    public partial class changeImageUrlLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DetailImage",
                table: "ProductPackage",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "详细介绍图片",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "详细介绍图片")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CardImage",
                table: "ProductPackage",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "产品卡片图片",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "产品卡片图片")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "Product",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "产品图标图片",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "产品图标图片")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DetailImage",
                table: "Product",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "产品详情介绍图片",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "产品详情介绍图片")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Product",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true,
                comment: "产品描述",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "产品描述")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DetailImage",
                table: "ProductPackage",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "详细介绍图片",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "详细介绍图片")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CardImage",
                table: "ProductPackage",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "产品卡片图片",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "产品卡片图片")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "Product",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "产品图标图片",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "产品图标图片")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DetailImage",
                table: "Product",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "产品详情介绍图片",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "产品详情介绍图片")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Product",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "产品描述",
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true,
                oldComment: "产品描述")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
