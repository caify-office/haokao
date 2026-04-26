using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LearnProgressService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearnProgressService.Infrastructure.Migrations
{
    public partial class ChangeCreatorNameType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<LearnProgress>(),
                    type: "varchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(40)",
                    oldNullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_LearnProgress_VideoId_ChapterId_CourseId_Identifier_CreatorI~",
                    table: GetShardingTableName<LearnProgress>(),
                    columns: new[] { "VideoId", "ChapterId", "CourseId", "Identifier", "CreatorId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_LearnProgress_VideoId_ChapterId_CourseId_Identifier_CreatorI~",
                    table: GetShardingTableName<LearnProgress>());
            }


            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "CreatorName",
                    table: GetShardingTableName<LearnProgress>(),
                    type: "nvarchar(40)",
                    nullable: true,
                    oldClrType: typeof(string),
                    oldType: "varchar(40)",
                    oldNullable: true)
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

        }
    }
}
