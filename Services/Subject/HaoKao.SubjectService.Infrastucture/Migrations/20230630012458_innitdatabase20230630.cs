using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.SubjectService.Infrastructure.Migrations
{
    public partial class innitdatabase20230630 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShow",
                table: "Subject",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShow",
                table: "Subject");
        }
    }
}
