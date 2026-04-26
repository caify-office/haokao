using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.KnowledgePointService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChpaterNodeName",
                table: "KnowledgePoint",
                newName: "ChapterNodeName");

            migrationBuilder.RenameColumn(
                name: "ChpaterNodeId",
                table: "KnowledgePoint",
                newName: "ChapterNodeId");

            migrationBuilder.RenameIndex(
                name: "IX_KnowledgePoint_ChpaterNodeId_Name_TenantId",
                table: "KnowledgePoint",
                newName: "IX_KnowledgePoint_ChapterNodeId_Name_TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChapterNodeName",
                table: "KnowledgePoint",
                newName: "ChpaterNodeName");

            migrationBuilder.RenameColumn(
                name: "ChapterNodeId",
                table: "KnowledgePoint",
                newName: "ChpaterNodeId");

            migrationBuilder.RenameIndex(
                name: "IX_KnowledgePoint_ChapterNodeId_Name_TenantId",
                table: "KnowledgePoint",
                newName: "IX_KnowledgePoint_ChpaterNodeId_Name_TenantId");
        }
    }
}
