using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ErrorCorrectingService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ErrorCorrectingService.Infrastructure.Migrations
{
    public partial class addIndex : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ErrorCorrecting>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ErrorCorrecting_SubjectId_CategoryId_QuestionTypeId",
                    table: GetShardingTableName<ErrorCorrecting>(),
                    columns: new[] { "SubjectId", "CategoryId", "QuestionTypeId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ErrorCorrecting>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_ErrorCorrecting_SubjectId_CategoryId_QuestionTypeId",
                    table: GetShardingTableName<ErrorCorrecting>());
            }

        }
    }
}
