using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CampaignService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CampaignService.Infrastructure.Migrations
{
    public partial class Initial : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<GiftBag>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<GiftBag>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CampaignName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "活动名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        GiftType = table.Column<int>(type: "int", nullable: false, comment: "礼包类型"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "产品Id", collation: "ascii_general_ci"),
                        ProductName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "产品名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "开始时间"),
                        EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "结束时间"),
                        IsPublished = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否发布"),
                        Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                        ReceiveCount = table.Column<int>(type: "int", nullable: false, comment: "领取数量"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "修改时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        WebSiteImageSet = table.Column<string>(type: "json", nullable: false, comment: "PC网站图片")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        WeChatMiniProgramImageSet = table.Column<string>(type: "json", nullable: false, comment: "微信小程序图片")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ReceiveRules = table.Column<string>(type: "json", nullable: false, comment: "领取规则")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_GiftBag", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<GiftBagReceiveLog>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<GiftBagReceiveLog>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        GiftBagId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "礼包Id", collation: "ascii_general_ci"),
                        CampaignName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "活动名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        GiftType = table.Column<int>(type: "int", nullable: false, comment: "礼包类型"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "产品Id", collation: "ascii_general_ci"),
                        ProductName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "产品名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ReceiveTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "领取时间"),
                        ReceiverId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "领取人Id", collation: "ascii_general_ci"),
                        ReceiverName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "领取人名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        RegistrationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_GiftBagReceiveLog", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<GiftBagReceiveLog>("FK_GiftBagReceiveLog_GiftBag_GiftBagId"),
                            column: x => x.GiftBagId,
                            principalTable: GetShardingTableName<GiftBag>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<GiftBag>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GiftBag_CampaignName_GiftType",
                    table: GetShardingTableName<GiftBag>(),
                    columns: new[] { "CampaignName", "GiftType" });
            }


            if (IsCreateShardingTable<GiftBagReceiveLog>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GiftBagReceiveLog_GiftBagId",
                    table: GetShardingTableName<GiftBagReceiveLog>(),
                    column: "GiftBagId");
            }


            if (IsCreateShardingTable<GiftBagReceiveLog>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_GiftBagReceiveLog_ReceiverName_ReceiveTime",
                    table: GetShardingTableName<GiftBagReceiveLog>(),
                    columns: new[] { "ReceiverName", "ReceiveTime" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<GiftBagReceiveLog>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<GiftBagReceiveLog>());
            }


            if (IsCreateShardingTable<GiftBag>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<GiftBag>());
            }

        }
    }
}
