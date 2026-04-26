using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LearnProgressService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearnProgressService.Infrastructure.Migrations
{
    public partial class addDailyStudyDuration : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DailyStudyDuration>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<DailyStudyDuration>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        DailyVideoStudyDuration = table.Column<double>(type: "double", nullable: false),
                        LearnTime = table.Column<DateOnly>(type: "date", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreatorName = table.Column<string>(type: "nvarchar(40)", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_DailyStudyDuration", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<DailyStudyDuration>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_DailyStudyDuration_ProductId_SubjectId_CreatorId",
                    table: GetShardingTableName<DailyStudyDuration>(),
                    columns: new[] { "ProductId", "SubjectId", "CreatorId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DailyStudyDuration>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<DailyStudyDuration>());
            }

        }
    }
}
