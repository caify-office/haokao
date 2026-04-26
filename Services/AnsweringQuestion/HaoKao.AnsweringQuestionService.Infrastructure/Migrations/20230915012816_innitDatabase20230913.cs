using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.AnsweringQuestionService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.AnsweringQuestionService.Infrastructure.Migrations
{
    public partial class innitDatabase20230913 : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AnsweringQuestion>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "ProductId",
                    table: GetShardingTableName<AnsweringQuestion>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<AnsweringQuestion>())
            {
                migrationBuilder.DropColumn(
                    name: "ProductId",
                    table: GetShardingTableName<AnsweringQuestion>());
            }

        }
    }
}
