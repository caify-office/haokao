using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    public partial class addHashType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashType",
                table: "AccessClientSecret",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 8, 22, 10, 35, 19, 428, DateTimeKind.Local).AddTicks(2640), new DateTime(2023, 8, 22, 10, 35, 19, 428, DateTimeKind.Local).AddTicks(2638) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashType",
                table: "AccessClientSecret");

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 8, 19, 17, 59, 39, 939, DateTimeKind.Local).AddTicks(5195), new DateTime(2023, 8, 19, 17, 59, 39, 939, DateTimeKind.Local).AddTicks(5193) });
        }
    }
}
