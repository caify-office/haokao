using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.CouponService.Infrastructure.Migrations
{
    public partial class _20240103innitdatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.DropColumn(
                    name: "Url",
                    table: GetShardingTableName<Coupon>());
            }


            if (IsCreateShardingTable<MarketingPersonnel>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<MarketingPersonnel>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "姓名")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TelPhone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "手机号码")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_MarketingPersonnel", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<MarketingPersonnel>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<MarketingPersonnel>());
            }


            if (IsCreateShardingTable<Coupon>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "Url",
                    table: GetShardingTableName<Coupon>(),
                    type: "varchar(500)",
                    maxLength: 500,
                    nullable: true,
                    comment: "生成的url链接地址")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
