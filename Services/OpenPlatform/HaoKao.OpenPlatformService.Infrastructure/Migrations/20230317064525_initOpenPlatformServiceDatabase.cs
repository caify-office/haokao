using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    public partial class initOpenPlatformServiceDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RegisterUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Phone = table.Column<string>(type: "varchar(11)", nullable: true, comment: "手机号码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(32)", nullable: true, comment: "密码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserGender = table.Column<int>(type: "int", nullable: false, comment: "用户性别"),
                    NickName = table.Column<string>(type: "varchar(30)", nullable: true, comment: "用户昵称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserState = table.Column<int>(type: "int", nullable: false, comment: "用户状态"),
                    EmailAddress = table.Column<string>(type: "varchar(40)", nullable: true, comment: "邮箱地址")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HeadImage = table.Column<string>(type: "varchar(200)", nullable: true, comment: "头像")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastLoginIp = table.Column<string>(type: "varchar(30)", nullable: true, comment: "最后登录IP")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastLoginTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后登录时间"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterUser", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExternalIdentity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Scheme = table.Column<string>(type: "varchar(20)", nullable: true, comment: "平台名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UniqueIdentifier = table.Column<string>(type: "varchar(100)", nullable: true, comment: "唯一标识")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OtherIdentifier = table.Column<string>(type: "varchar(100)", nullable: true, comment: "其它标识")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OtherInformation = table.Column<string>(type: "varchar(1000)", nullable: true, comment: "其它信息")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegisterUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalIdentity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalIdentity_RegisterUser_RegisterUserId",
                        column: x => x.RegisterUserId,
                        principalTable: "RegisterUser",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentity_RegisterUserId",
                table: "ExternalIdentity",
                column: "RegisterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentity_UniqueIdentifier_Scheme_OtherIdentifier",
                table: "ExternalIdentity",
                columns: new[] { "UniqueIdentifier", "Scheme", "OtherIdentifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegisterUser_Phone",
                table: "RegisterUser",
                column: "Phone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalIdentity");

            migrationBuilder.DropTable(
                name: "RegisterUser");
        }
    }
}
