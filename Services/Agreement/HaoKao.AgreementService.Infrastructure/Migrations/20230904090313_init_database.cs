using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.AgreementService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.AgreementService.Infrastructure.Migrations
{
    public partial class init_database : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<CourseAgreement>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<CourseAgreement>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "协议名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Content = table.Column<string>(type: "text", nullable: false, comment: "协议内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Continuation = table.Column<int>(type: "int", nullable: false, comment: "续读次数"),
                        AgreementType = table.Column<int>(type: "int", nullable: false, comment: "协议类型"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "创建人")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_CourseAgreement", x => x.Id);
                    },
                    comment: "课程协议")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<StudentAgreement>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<StudentAgreement>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "产品Id", collation: "ascii_general_ci"),
                        ProductName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "产品名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AgreementId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "协议Id", collation: "ascii_general_ci"),
                        AgreementName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "协议名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        StudentName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "学员名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IdCard = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false, comment: "身份证号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Contact = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "联系电话")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Address = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "联系地址")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "电子邮箱")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "签署人Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_StudentAgreement", x => x.Id);
                    },
                    comment: "学员协议")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<StudentAgreement>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_StudentAgreement_ProductId_AgreementId_CreatorId_TenantId",
                    table: GetShardingTableName<StudentAgreement>(),
                    columns: new[] { "ProductId", "AgreementId", "CreatorId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseAgreement>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<CourseAgreement>());
            }


            if (IsCreateShardingTable<StudentAgreement>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<StudentAgreement>());
            }

        }
    }
}