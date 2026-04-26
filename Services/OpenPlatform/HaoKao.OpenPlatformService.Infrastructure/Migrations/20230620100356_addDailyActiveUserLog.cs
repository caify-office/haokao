using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    public partial class addDailyActiveUserLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyActiveUserLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "用户Id", collation: "ascii_general_ci"),
                    ClientId = table.Column<string>(type: "varchar(50)", nullable: false, comment: "活跃的客户端Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<string>(type: "varchar(20)", nullable: false, comment: "活跃的日期(yyyy-MM-dd)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyActiveUserLog", x => x.Id);
                },
                comment: "用户每日活跃记录")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 6, 20, 18, 3, 55, 914, DateTimeKind.Local).AddTicks(2650), new DateTime(2023, 6, 20, 18, 3, 55, 914, DateTimeKind.Local).AddTicks(2640) });

            migrationBuilder.CreateIndex(
                name: "IX_DailyActiveUserLog_UserId_ClientId_CreateDate_CreateTime",
                table: "DailyActiveUserLog",
                columns: new[] { "UserId", "ClientId", "CreateDate", "CreateTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyActiveUserLog");

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 6, 20, 12, 0, 48, 314, DateTimeKind.Local).AddTicks(9250), new DateTime(2023, 6, 20, 12, 0, 48, 314, DateTimeKind.Local).AddTicks(9240) });
        }
    }
}
