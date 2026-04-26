using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionCategoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionCategoryService.Infrastructure.Migrations
{
    public partial class innitDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<QuestionCategory>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "类名名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "类别代码")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_QuestionCategory", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_QuestionCategory_Name_TenantId",
                    table: GetShardingTableName<QuestionCategory>(),
                    columns: new[] { "Name", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCategory>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<QuestionCategory>());
            }

        }
    }
}
