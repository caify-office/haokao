using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.PaperService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.PaperService.Infrastructure.Migrations
{
    public partial class innitdatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<Paper>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Paper>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "试卷名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "科目名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CategoryName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "分类名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Time = table.Column<int>(type: "int", nullable: false),
                        Score = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "及格分数"),
                        IsFree = table.Column<int>(type: "int", nullable: false),
                        State = table.Column<int>(type: "int", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                        StructJson = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Paper", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Paper>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Paper_SubjectId_CategoryId_TenantId",
                    table: GetShardingTableName<Paper>(),
                    columns: new[] { "SubjectId", "CategoryId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Paper>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Paper>());
            }

        }
    }
}
