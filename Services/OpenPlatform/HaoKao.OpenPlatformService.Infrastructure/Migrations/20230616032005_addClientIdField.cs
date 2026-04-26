using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    public partial class addClientIdField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "RegisterUser",
                type: "longtext",
                nullable: true,
                comment: "客户端Id")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 6, 16, 11, 20, 5, 783, DateTimeKind.Local).AddTicks(8800), new DateTime(2023, 6, 16, 11, 20, 5, 783, DateTimeKind.Local).AddTicks(8800) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "RegisterUser");

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 6, 7, 11, 4, 0, 836, DateTimeKind.Local).AddTicks(2490), new DateTime(2023, 6, 7, 11, 4, 0, 836, DateTimeKind.Local).AddTicks(2490) });
        }
    }
}
