using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    public partial class addUserAccountField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegisterUser_Phone",
                table: "RegisterUser");

            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "RegisterUser",
                type: "varchar(30)",
                nullable: true,
                comment: "用户账号")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterUser_Phone_Account_NickName_UserState_CreateTime",
                table: "RegisterUser",
                columns: new[] { "Phone", "Account", "NickName", "UserState", "CreateTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegisterUser_Phone_Account_NickName_UserState_CreateTime",
                table: "RegisterUser");

            migrationBuilder.DropColumn(
                name: "Account",
                table: "RegisterUser");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterUser_Phone",
                table: "RegisterUser",
                column: "Phone");
        }
    }
}
