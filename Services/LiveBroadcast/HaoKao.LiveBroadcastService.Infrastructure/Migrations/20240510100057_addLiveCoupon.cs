using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LiveBroadcastService.Infrastructure.Migrations
{
    public partial class addLiveCoupon : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LiveCoupon>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<LiveCoupon>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        LiveVideoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        LiveCouponId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        LiveCouponName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "优惠卷名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Amount = table.Column<decimal>(type: "decimal(18,2)", maxLength: 20, precision: 18, scale: 2, nullable: false, comment: "金额/折扣--合并一个字段  折扣85折显示0.85"),
                        CouponType = table.Column<int>(type: "int", nullable: false),
                        Scope = table.Column<int>(type: "int", nullable: false),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        IsShelves = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        Sort = table.Column<int>(type: "int", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_LiveCoupon", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<LiveCoupon>("FK_LiveCoupon_LiveVideo_LiveVideoId"),
                            column: x => x.LiveVideoId,
                            principalTable: GetShardingTableName<LiveVideo>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LiveCoupon>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LiveCoupon_LiveVideoId",
                    table: GetShardingTableName<LiveCoupon>(),
                    column: "LiveVideoId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LiveCoupon>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<LiveCoupon>());
            }

        }
    }
}
