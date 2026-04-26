using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CourseRatingService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CourseRatingService.Infrastructure.Migrations
{
    public partial class initDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<CourseRating>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<CourseRating>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CourseId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "课程Id", collation: "ascii_general_ci"),
                        CourseName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "课程名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Comment = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false, comment: "评价内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Rating = table.Column<int>(type: "int", nullable: false, comment: "评价级别"),
                        AuditState = table.Column<int>(type: "int", nullable: false, comment: "审核状态"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "评价时间"),
                        NickName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "昵称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "评价人")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Sticky = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否置顶"),
                        Avatar = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false, comment: "头像")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_CourseRating", x => x.Id);
                    },
                    comment: "课程评价")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<CourseRating>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_CourseRating_CourseId_CreatorId_TenantId",
                    table: GetShardingTableName<CourseRating>(),
                    columns: new[] { "CourseId", "CreatorId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<CourseRating>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<CourseRating>());
            }

        }
    }
}