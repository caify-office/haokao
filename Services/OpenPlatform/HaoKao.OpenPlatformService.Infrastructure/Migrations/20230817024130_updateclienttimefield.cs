using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    public partial class updateclienttimefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "AccessClient");

            migrationBuilder.DropColumn(
                name: "LastAccessed",
                table: "AccessClient");

            migrationBuilder.DropColumn(
                name: "NonEditable",
                table: "AccessClient");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "AccessClient");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "AccessClient",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "AccessClient",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 8, 17, 10, 41, 30, 290, DateTimeKind.Local).AddTicks(5920), new DateTime(2023, 8, 17, 10, 41, 30, 290, DateTimeKind.Local).AddTicks(5920) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "AccessClient");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "AccessClient");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "AccessClient",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessed",
                table: "AccessClient",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonEditable",
                table: "AccessClient",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "AccessClient",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 8, 17, 10, 25, 1, 403, DateTimeKind.Local).AddTicks(1110), new DateTime(2023, 8, 17, 10, 25, 1, 403, DateTimeKind.Local).AddTicks(1080) });
        }
    }
}
