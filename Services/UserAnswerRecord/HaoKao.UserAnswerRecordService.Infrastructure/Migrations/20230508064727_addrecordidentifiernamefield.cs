using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Migrations
{
    public partial class addrecordidentifiernamefield : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserAnswerRecord>())
            {
                migrationBuilder.AddColumn<string>(
                    name: "RecordIdentifierName",
                    table: GetShardingTableName<UserAnswerRecord>(),
                    type: "varchar(100)",
                    nullable: true,
                    comment: "答题标识符名称")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserAnswerRecord>())
            {
                migrationBuilder.DropColumn(
                    name: "RecordIdentifierName",
                    table: GetShardingTableName<UserAnswerRecord>());
            }

        }
    }
}
