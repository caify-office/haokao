using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LiveBroadcastService.Infrastructure.Migrations
{
    public partial class InitDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<LiveAdministrator>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LiveAdministrator>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "姓名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LiveAdministrator", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveAnnouncement>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LiveAnnouncement>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "标题")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Content = table.Column<string>(type: "text", nullable: true, comment: "内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LiveAnnouncement", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveComment>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LiveComment>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Rating = table.Column<int>(type: "int", nullable: false),
                        Content = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "评价内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        LiveId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LiveComment", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveMessage>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LiveMessage>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        LiveId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "直播间Id", collation: "ascii_general_ci"),
                        Content = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "消息内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        LiveMessageType = table.Column<int>(type: "int", nullable: false, comment: "消息类型"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true, comment: "用户名称"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "发送时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LiveMessage", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveOnlineUser>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LiveOnlineUser>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        LiveId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "直播间Id", collation: "ascii_general_ci"),
                        Phone = table.Column<string>(type: "longtext", nullable: true, comment: "手机号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsOnline = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否在线"),
                        OnlineDuration = table.Column<int>(type: "int", nullable: false, comment: "累计在线时长"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "用户Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true, comment: "用户名称"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "首次上线时间"),
                        LastOnlineTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后上线时间")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LiveOnlineUser", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveOnlineUserTrend>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LiveOnlineUserTrend>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        LiveId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "直播Id", collation: "ascii_general_ci"),
                        Interval = table.Column<int>(type: "int", nullable: false, comment: "统计间隔(分钟)"),
                        TotalCount = table.Column<int>(type: "int", nullable: false, comment: "累计在线人数"),
                        OnlineCount = table.Column<int>(type: "int", nullable: false, comment: "当前在线人数"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "统计时间"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LiveOnlineUserTrend", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveReservation>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LiveReservation>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        LiveVideoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ReservationSource = table.Column<int>(type: "int", nullable: false),
                        Notified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        OpenId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "OpenId")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LiveReservation", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveVideo>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LiveVideo>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CardUrl = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true, comment: "卡片")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        LiveType = table.Column<int>(type: "int", nullable: false),
                        SubjectIds = table.Column<string>(type: "text", nullable: true, comment: "科目Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SubjectIdsStr = table.Column<string>(type: "text", nullable: true, comment: "科目Id字符串")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SubjectNames = table.Column<string>(type: "text", nullable: true, comment: "科目名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        LecturerId = table.Column<string>(type: "text", nullable: true, comment: "主讲老师Id")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        LecturerName = table.Column<string>(type: "text", nullable: true, comment: "主讲老师名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        StartTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "直播开始时间"),
                        EndTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "直播结束时间"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        LiveStatus = table.Column<int>(type: "int", nullable: false),
                        TargetProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Desc = table.Column<string>(type: "text", nullable: true, comment: "详情介绍")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        LiveAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true, comment: "播流地址")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        StreamingAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true, comment: "推流地址")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        LiveAnnouncementId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                        IsUploadPlayBack = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LiveVideo", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<MutedUser>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<MutedUser>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "用户名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_MutedUser", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<SensitiveWord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<SensitiveWord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Content = table.Column<string>(type: "text", nullable: true, comment: "内容")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_SensitiveWord", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LivePlayBack>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LivePlayBack>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        LiveVideoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        VideoType = table.Column<int>(type: "int", nullable: false),
                        Duration = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "视频时长"),
                        VideoNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "视频序号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        Sort = table.Column<int>(type: "int", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LivePlayBack", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<LivePlayBack>("FK_LivePlayBack_LiveVideo_LiveVideoId"),
                            column: x => x.LiveVideoId,
                            principalTable: GetShardingTableName<LiveVideo>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveProductPackage>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LiveProductPackage>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        LiveVideoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ProductPackageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ProductPackageName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "产品包名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        ProductType = table.Column<int>(type: "int", nullable: false),
                        Sort = table.Column<int>(type: "int", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LiveProductPackage", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<LiveProductPackage>("FK_LiveProductPackage_LiveVideo_LiveVideoId"),
                            column: x => x.LiveVideoId,
                            principalTable: GetShardingTableName<LiveVideo>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveAdministrator>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LiveAdministrator_UserId_Phone_Name",
                    table: GetShardingTableName<LiveAdministrator>(),
                    columns: new[] { "UserId", "Phone", "Name" });
            }


            if (IsCreateShardingTable<LiveComment>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LiveComment_Phone_LiveId_TenantId",
                    table: GetShardingTableName<LiveComment>(),
                    columns: new[] { "Phone", "LiveId", "TenantId" });
            }


            if (IsCreateShardingTable<LiveMessage>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LiveMessage_LiveId_CreatorId_TenantId",
                    table: GetShardingTableName<LiveMessage>(),
                    columns: new[] { "LiveId", "CreatorId", "TenantId" });
            }


            if (IsCreateShardingTable<LiveOnlineUser>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LiveOnlineUser_LiveId_CreatorId_TenantId",
                    table: GetShardingTableName<LiveOnlineUser>(),
                    columns: new[] { "LiveId", "CreatorId", "TenantId" });
            }


            if (IsCreateShardingTable<LiveOnlineUserTrend>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LiveOnlineUserTrend_LiveId_TenantId",
                    table: GetShardingTableName<LiveOnlineUserTrend>(),
                    columns: new[] { "LiveId", "TenantId" });
            }


            if (IsCreateShardingTable<LivePlayBack>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LivePlayBack_LiveVideoId",
                    table: GetShardingTableName<LivePlayBack>(),
                    column: "LiveVideoId");
            }


            if (IsCreateShardingTable<LiveProductPackage>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LiveProductPackage_LiveVideoId",
                    table: GetShardingTableName<LiveProductPackage>(),
                    column: "LiveVideoId");
            }


            if (IsCreateShardingTable<LiveReservation>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LiveReservation_CreatorId_Phone_OpenId_ProductId_LiveVideoId~",
                    table: GetShardingTableName<LiveReservation>(),
                    columns: new[] { "CreatorId", "Phone", "OpenId", "ProductId", "LiveVideoId", "TenantId" });
            }


            if (IsCreateShardingTable<LiveVideo>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LiveVideo_LiveStatus_LiveType_Name",
                    table: GetShardingTableName<LiveVideo>(),
                    columns: new[] { "LiveStatus", "LiveType", "Name" });
            }


            if (IsCreateShardingTable<MutedUser>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_MutedUser_UserId_TenantId",
                    table: GetShardingTableName<MutedUser>(),
                    columns: new[] { "UserId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LiveAdministrator>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LiveAdministrator>());
            }


            if (IsCreateShardingTable<LiveAnnouncement>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LiveAnnouncement>());
            }


            if (IsCreateShardingTable<LiveComment>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LiveComment>());
            }


            if (IsCreateShardingTable<LiveMessage>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LiveMessage>());
            }


            if (IsCreateShardingTable<LiveOnlineUser>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LiveOnlineUser>());
            }


            if (IsCreateShardingTable<LiveOnlineUserTrend>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LiveOnlineUserTrend>());
            }


            if (IsCreateShardingTable<LivePlayBack>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LivePlayBack>());
            }


            if (IsCreateShardingTable<LiveProductPackage>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LiveProductPackage>());
            }


            if (IsCreateShardingTable<LiveReservation>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LiveReservation>());
            }


            if (IsCreateShardingTable<MutedUser>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<MutedUser>());
            }


            if (IsCreateShardingTable<SensitiveWord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<SensitiveWord>());
            }


            if (IsCreateShardingTable<LiveVideo>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LiveVideo>());
            }

        }
    }
}
