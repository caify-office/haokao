using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.AnsweringQuestionService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.AnsweringQuestionService.Infrastructure.Migrations
{
    public partial class innitdatabase20230831 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<AnsweringQuestion>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<AnsweringQuestion>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                        UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "用户名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号码")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "科目名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsReply = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CourseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CourseName = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CourseChapterId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CourseChapterName = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CourseVideId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CourseVideName = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        BookPageSize = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "书籍页码")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        BookName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "书籍名称以及相关描述")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Type = table.Column<long>(type: "bigint", nullable: false),
                        Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "问题描述")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Remark = table.Column<string>(type: "longtext", nullable: true, comment: "详细描述")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        FileUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "上传的图片路劲")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        WatchCount = table.Column<int>(type: "int", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_AnsweringQuestion", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<AnsweringQuestion>("FK_AnsweringQuestion_AnsweringQuestion_ParentId"),
                            column: x => x.ParentId,
                            principalTable: GetShardingTableName<AnsweringQuestion>(),
                            principalColumn: "Id");
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<AnsweringQuestionReply>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<AnsweringQuestionReply>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ReplyContent = table.Column<string>(type: "longtext", nullable: true, comment: "答疑回复内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ReplyUserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "回复人用户名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AnsweringQuestionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_AnsweringQuestionReply", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<AnsweringQuestionReply>("FK_AnsweringQuestionReply_AnsweringQuestion_AnsweringQuestionId"),
                            column: x => x.AnsweringQuestionId,
                            principalTable: GetShardingTableName<AnsweringQuestion>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<AnsweringQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AnsweringQuestion_ParentId",
                    table: GetShardingTableName<AnsweringQuestion>(),
                    column: "ParentId");
            }


            if (IsCreateShardingTable<AnsweringQuestionReply>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AnsweringQuestionReply_AnsweringQuestionId",
                    table: GetShardingTableName<AnsweringQuestionReply>(),
                    column: "AnsweringQuestionId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AnsweringQuestionReply>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<AnsweringQuestionReply>());
            }


            if (IsCreateShardingTable<AnsweringQuestion>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<AnsweringQuestion>());
            }

        }
    }
}
