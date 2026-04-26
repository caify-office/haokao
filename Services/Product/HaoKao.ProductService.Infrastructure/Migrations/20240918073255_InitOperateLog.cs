using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitOperateLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentPermissionOperateLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StudentPermissionId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "学员权限Id", collation: "ascii_general_ci"),
                    StudentName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "学员昵称(即用户昵称)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StudentId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "学员ID（即用户ID）", collation: "ascii_general_ci"),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "产品Id", collation: "ascii_general_ci"),
                    ProductName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "产品名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductType = table.Column<int>(type: "int", nullable: false, comment: "产品类型"),
                    OldExpiredTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "原过期时间"),
                    NewExpiredTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "现过期时间"),
                    CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "创建人Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPermissionOperateLog", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPermissionOperateLog_CreateTime_StudentName",
                table: "StudentPermissionOperateLog",
                columns: new[] { "CreateTime", "StudentName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentPermissionOperateLog");
        }
    }
}
