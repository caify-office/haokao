using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Migrations
{
    public partial class addParentId : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "ParentId",
                    table: GetShardingTableName<UserAnswerQuestion>(),
                    type: "char(36)",
                    nullable: true,
                    comment: "父题目Id",
                    collation: "ascii_general_ci");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UserAnswerQuestion>())
            {
                migrationBuilder.DropColumn(
                    name: "ParentId",
                    table: GetShardingTableName<UserAnswerQuestion>());
            }
        }
    }
}