using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addAssistantProductPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssistantProductPermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubjectName = table.Column<string>(type: "varchar(50)", nullable: true, comment: "对应科目名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CourseStageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CourseStageName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "课程阶段名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AssistantProductPermissionContents = table.Column<string>(type: "json", nullable: true, comment: "智辅产品权限内容")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssistantProductPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssistantProductPermission_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AssistantProductPermission_ProductId",
                table: "AssistantProductPermission",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistantProductPermission_SubjectId_TenantId",
                table: "AssistantProductPermission",
                columns: new[] { "SubjectId", "TenantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssistantProductPermission");
        }
    }
}
