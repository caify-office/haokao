using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ChapterNodeService.Infrastructure.Migrations
{
    public partial class addPropertyValueID : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "PropertyValueID",
                    table: GetShardingTableName<ChapterNode>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "从题库迁移的数据")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.DropColumn(
                    name: "PropertyValueID",
                    table: GetShardingTableName<ChapterNode>());
            }

        }
    }
}
