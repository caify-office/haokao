using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ChapterNodeService.Infrastructure.Migrations
{
    public partial class addKnowledgePointSort : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<KnowledgePoint>())
            {
                migrationBuilder.AddColumn<int>(
                    name: "Sort",
                    table: GetShardingTableName<KnowledgePoint>(),
                    type: "int",
                    nullable: false,
                    defaultValue: 0);
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<KnowledgePoint>())
            {
                migrationBuilder.DropColumn(
                    name: "Sort",
                    table: GetShardingTableName<KnowledgePoint>());
            }

        }
    }
}
