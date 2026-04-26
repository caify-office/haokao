using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.StudyMaterialService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.StudyMaterialService.Infrastructure.Migrations
{
    public partial class initDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<StudyMaterial>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<StudyMaterial>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "资料名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Year = table.Column<int>(type: "int", nullable: false, comment: "年份"),
                        Enable = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "启用/禁用"),
                        Materials = table.Column<string>(type: "json", nullable: false, comment: "资料内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Subjects = table.Column<string>(type: "text", nullable: false, comment: "科目")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "创建人")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_StudyMaterial", x => x.Id);
                    },
                    comment: "学习资料")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<StudyMaterial>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<StudyMaterial>());
            }

        }
    }
}