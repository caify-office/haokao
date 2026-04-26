using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.FeedBackService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.FeedBackService.Infrastructure.Migrations
{
    public partial class _20231213innitdatabase8 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<FeedBack>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<FeedBack>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Type = table.Column<int>(type: "int", nullable: false),
                        SourceType = table.Column<int>(type: "int", nullable: false),
                        Status = table.Column<int>(type: "int", nullable: false),
                        Contract = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "联系人")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true, comment: "描述")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        FileUrls = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "图片")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_FeedBack", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<FeedBack>("FK_FeedBack_FeedBack_ParentId"),
                            column: x => x.ParentId,
                            principalTable: GetShardingTableName<FeedBack>(),
                            principalColumn: "Id");
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<FeedBackReply>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<FeedBackReply>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ReplyContent = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "答疑回复内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ReplyUserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "回复人用户名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        FeedBackId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_FeedBackReply", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<FeedBackReply>("FK_FeedBackReply_FeedBack_FeedBackId"),
                            column: x => x.FeedBackId,
                            principalTable: GetShardingTableName<FeedBack>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<FeedBack>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_FeedBack_ParentId",
                    table: GetShardingTableName<FeedBack>(),
                    column: "ParentId");
            }


            if (IsCreateShardingTable<FeedBackReply>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_FeedBackReply_FeedBackId",
                    table: GetShardingTableName<FeedBackReply>(),
                    column: "FeedBackId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<FeedBackReply>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<FeedBackReply>());
            }


            if (IsCreateShardingTable<FeedBack>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<FeedBack>());
            }

        }
    }
}
