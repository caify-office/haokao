using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    public partial class deleteOtherIndentityField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExternalIdentity_UniqueIdentifier_Scheme_OtherIdentifier",
                table: "ExternalIdentity");

            migrationBuilder.DropColumn(
                name: "OtherIdentifier",
                table: "ExternalIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentity_UniqueIdentifier_Scheme",
                table: "ExternalIdentity",
                columns: new[] { "UniqueIdentifier", "Scheme" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExternalIdentity_UniqueIdentifier_Scheme",
                table: "ExternalIdentity");

            migrationBuilder.AddColumn<string>(
                name: "OtherIdentifier",
                table: "ExternalIdentity",
                type: "varchar(100)",
                nullable: true,
                comment: "其它标识")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentity_UniqueIdentifier_Scheme_OtherIdentifier",
                table: "ExternalIdentity",
                columns: new[] { "UniqueIdentifier", "Scheme", "OtherIdentifier" },
                unique: true);
        }
    }
}
