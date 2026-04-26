using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.FeedBackService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.FeedBackService.Infrastructure.Migrations
{
    public partial class InitSuggestion : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Suggestion>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Suggestion>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SuggestionType = table.Column<int>(type: "int", nullable: false, comment: "反馈类型"),
                        SuggestionFrom = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "反馈来源")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "问题描述")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Screenshots = table.Column<string>(type: "json", nullable: true, comment: "相关截图")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "反馈人Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "反馈人名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "反馈时间")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ReplyState = table.Column<int>(type: "int", nullable: false, comment: "处理状态"),
                        ReplyUserId = table.Column<Guid>(type: "char(36)", nullable: true, comment: "处理人Id", collation: "ascii_general_ci"),
                        ReplyUserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "处理人名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ReplyTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "回复时间"),
                        ReplyContent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "回复内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ReplyScreenshots = table.Column<string>(type: "json", nullable: true, comment: "回复截图")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Suggestion", x => x.Id);
                    },
                    comment: "意见反馈")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Suggestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Suggestion_Phone_CreatorId_TenantId",
                    table: GetShardingTableName<Suggestion>(),
                    columns: new[] { "Phone", "CreatorId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Suggestion>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Suggestion>());
            }

        }
    }
}
