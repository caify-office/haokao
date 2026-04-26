using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    public partial class editSigningAlgorithms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedIdentityTokenSigningAlgorithms",
                table: "AccessClient");

            migrationBuilder.CreateTable(
                name: "AccessClientSigningAlgorithm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccessClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SigningAlgorithm = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessClientSigningAlgorithm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessClientSigningAlgorithm_AccessClient_AccessClientId",
                        column: x => x.AccessClientId,
                        principalTable: "AccessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 8, 19, 17, 59, 39, 939, DateTimeKind.Local).AddTicks(5195), new DateTime(2023, 8, 19, 17, 59, 39, 939, DateTimeKind.Local).AddTicks(5193) });

            migrationBuilder.CreateIndex(
                name: "IX_AccessClientSigningAlgorithm_AccessClientId",
                table: "AccessClientSigningAlgorithm",
                column: "AccessClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessClientSigningAlgorithm");

            migrationBuilder.AddColumn<string>(
                name: "AllowedIdentityTokenSigningAlgorithms",
                table: "AccessClient",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 8, 18, 10, 12, 48, 146, DateTimeKind.Local).AddTicks(7740), new DateTime(2023, 8, 18, 10, 12, 48, 146, DateTimeKind.Local).AddTicks(7740) });
        }
    }
}
