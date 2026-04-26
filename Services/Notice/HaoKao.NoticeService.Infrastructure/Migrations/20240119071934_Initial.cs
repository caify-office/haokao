using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.NoticeService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.NoticeService.Infrastructure.Migrations
{
    public partial class Initial : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<Notice>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Notice>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Title = table.Column<string>(type: "longtext", nullable: false, comment: "公告名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Content = table.Column<string>(type: "longtext", nullable: false, comment: "公告内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Published = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否发布"),
                        Popup = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否弹出"),
                        StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "弹出开始时间"),
                        EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "弹出结束时间"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "创建人Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true, comment: "创建人名称"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Notice", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Notice>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Notice>());
            }

        }
    }
}