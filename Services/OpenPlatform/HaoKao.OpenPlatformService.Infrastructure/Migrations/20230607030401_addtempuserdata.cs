using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    public partial class addtempuserdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RegisterUser",
                columns: new[] { "Id", "Account", "CreateTime", "EmailAddress", "HeadImage", "LastLoginIp", "LastLoginTime", "NickName", "Password", "Phone", "UserGender", "UserState" },
                values: new object[] { new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"), "tempuser", new DateTime(2023, 6, 7, 11, 4, 0, 836, DateTimeKind.Local).AddTicks(2490), "13000000000@qq.com", null, null, new DateTime(2023, 6, 7, 11, 4, 0, 836, DateTimeKind.Local).AddTicks(2490), "临时用户", "E10ADC3949BA59ABBE56E057F20F883E", "13000000000", 0, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"));
        }
    }
}
