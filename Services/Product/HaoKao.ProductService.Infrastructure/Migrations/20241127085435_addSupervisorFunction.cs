using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addSupervisorFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupervisorClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "班级名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "产品名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SalespersonId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "营销人员Id", collation: "ascii_general_ci"),
                    SalespersonName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "营销人员名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervisorClass", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SupervisorStudent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SupervisorClassId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RegisterUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "昵称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxProgress = table.Column<float>(type: "float", nullable: false),
                    CourseDuration = table.Column<float>(type: "float", nullable: false),
                    CourseRatio = table.Column<float>(type: "float", nullable: false),
                    IsEndCourseCount = table.Column<int>(type: "int", nullable: false),
                    CourseCount = table.Column<int>(type: "int", nullable: false),
                    LastLearnTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最近学习时间"),
                    StatisticsTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "统计时间 （每一天只统计一次）"),
                    TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervisorStudent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupervisorStudent_SupervisorClass_SupervisorClassId",
                        column: x => x.SupervisorClassId,
                        principalTable: "SupervisorClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorClass_ProductId_Year_TenantId",
                table: "SupervisorClass",
                columns: new[] { "ProductId", "Year", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorStudent_SupervisorClassId",
                table: "SupervisorStudent",
                column: "SupervisorClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupervisorStudent");

            migrationBuilder.DropTable(
                name: "SupervisorClass");
        }
    }
}
