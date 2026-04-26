using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.BasicService.Infrastructure.Migrations
{
    public partial class ChangeColumnType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Role",
                type: "varchar(30)",
                nullable: true,
                comment: "角色名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldNullable: true,
                oldComment: "角色名称")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Desc",
                table: "Role",
                type: "varchar(200)",
                nullable: true,
                comment: "角色描述",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true,
                oldComment: "角色描述")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("58205e0e-1552-4282-bedc-a92d0afb37df"),
                column: "CreateTime",
                value: new DateTime(2024, 4, 16, 14, 14, 4, 686, DateTimeKind.Local).AddTicks(8180));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Role",
                type: "nvarchar(30)",
                nullable: true,
                comment: "角色名称",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true,
                oldComment: "角色名称")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Desc",
                table: "Role",
                type: "nvarchar(200)",
                nullable: true,
                comment: "角色描述",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true,
                oldComment: "角色描述")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("58205e0e-1552-4282-bedc-a92d0afb37df"),
                column: "CreateTime",
                value: new DateTime(2023, 2, 24, 11, 53, 23, 547, DateTimeKind.Local).AddTicks(2290));
        }
    }
}
