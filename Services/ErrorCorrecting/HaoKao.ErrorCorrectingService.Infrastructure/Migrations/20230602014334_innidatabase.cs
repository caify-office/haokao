using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ErrorCorrectingService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ErrorCorrectingService.Infrastructure.Migrations
{
    public partial class innidatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<ErrorCorrecting>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<ErrorCorrecting>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        QuestionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true, comment: "描述")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        QuestionTypes = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "问题类型")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectName = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        QuestionTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        QuestionTypeName = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        QuestionText = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        NickName = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Phone = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CategoryName = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ErrorCorrecting", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ErrorCorrecting>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<ErrorCorrecting>());
            }

        }
    }
}
