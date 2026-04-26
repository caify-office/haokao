using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterOldExpiredTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OldExpiredTime",
                table: "StudentPermissionOperateLog",
                type: "datetime(6)",
                nullable: true,
                comment: "原过期时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "原过期时间");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OldExpiredTime",
                table: "StudentPermissionOperateLog",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "原过期时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldComment: "原过期时间");
        }
    }
}
