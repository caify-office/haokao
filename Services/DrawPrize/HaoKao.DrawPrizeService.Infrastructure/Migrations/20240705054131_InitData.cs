using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DrawPrizeService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DrawPrizeService.Infrastructure.Migrations
{
    public partial class InitData : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<DrawPrize>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        BackgroundImageUrl = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "活动背景图")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        DrawPrizeRange = table.Column<int>(type: "int", nullable: false),
                        DrawPrizeType = table.Column<int>(type: "int", nullable: false),
                        StartTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "开始时间"),
                        EndTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "结束时间"),
                        Desc = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true, comment: "说明")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Probability = table.Column<double>(type: "double", nullable: false),
                        Enable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        DrawPrizeUrl = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "抽奖链接")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_DrawPrize", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<DrawPrizeRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<DrawPrizeRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        DrawPrizeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true),
                        Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PrizeName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "奖品名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_DrawPrizeRecord", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<DrawPrizeRecord>("FK_DrawPrizeRecord_DrawPrize_DrawPrizeId"),
                            column: x => x.DrawPrizeId,
                            principalTable: GetShardingTableName<DrawPrize>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<Prize>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<Prize>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        DrawPrizeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Count = table.Column<int>(type: "int", nullable: false),
                        DisplayCount = table.Column<int>(type: "int", nullable: false),
                        WinningRange = table.Column<int>(type: "int", nullable: false),
                        DesignatedStudents = table.Column<string>(type: "json", nullable: true, comment: "指定学员")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsGuaranteed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        AwardedQuantity = table.Column<int>(type: "int", nullable: false),
                        Version = table.Column<DateTime>(type: "datetime(6)", rowVersion: true, nullable: false)
                            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Prize", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<Prize>("FK_Prize_DrawPrize_DrawPrizeId"),
                            column: x => x.DrawPrizeId,
                            principalTable: GetShardingTableName<DrawPrize>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_DrawPrize_DrawPrizeType",
                    table: GetShardingTableName<DrawPrize>(),
                    column: "DrawPrizeType");
            }


            if (IsCreateShardingTable<DrawPrizeRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_DrawPrizeRecord_DrawPrizeId",
                    table: GetShardingTableName<DrawPrizeRecord>(),
                    column: "DrawPrizeId");
            }


            if (IsCreateShardingTable<Prize>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Prize_DrawPrizeId",
                    table: GetShardingTableName<Prize>(),
                    column: "DrawPrizeId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DrawPrizeRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<DrawPrizeRecord>());
            }


            if (IsCreateShardingTable<Prize>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<Prize>());
            }


            if (IsCreateShardingTable<DrawPrize>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<DrawPrize>());
            }

        }
    }
}
