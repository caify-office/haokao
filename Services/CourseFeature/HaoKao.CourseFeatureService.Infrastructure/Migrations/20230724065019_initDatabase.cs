using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseFeatureService.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseFeatureService.Infrastructure.Migrations
{
    public partial class initDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<CourseFeature>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<CourseFeature>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "服务名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Content = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "服务内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IconUrl = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "图标地址")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Enable = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "启用/禁用"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "创建人")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_CourseFeature", x => x.Id);
                    },
                    comment: "课程特色服务")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseFeature>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<CourseFeature>());
            }

        }
    }
}