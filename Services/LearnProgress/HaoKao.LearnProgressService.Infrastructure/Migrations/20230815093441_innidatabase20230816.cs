using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LearnProgressService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearnProgressService.Infrastructure.Migrations
{
    public partial class innidatabase20230816 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LearnProgress>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ChapterId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CourseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        VideoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Identifier = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "当前视频标识符,")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Progress = table.Column<int>(type: "int", nullable: false),
                        TotalProgress = table.Column<int>(type: "int", nullable: false),
                        MaxProgress = table.Column<int>(type: "int", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LearnProgress", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LearnProgress>());
            }

        }
    }
}
