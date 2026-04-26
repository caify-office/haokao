using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LecturerService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LecturerService.Infrastructure.Migrations
{
    public partial class InitDataBase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Lecturer>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "教师姓名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Desc = table.Column<string>(type: "text", nullable: true, comment: "简介")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        HeadPortraitUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "头像")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PhotoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "形象照")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Sort = table.Column<int>(type: "int", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Lecturer", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Lecturer>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Lecturer>());
            }

        }
    }
}
