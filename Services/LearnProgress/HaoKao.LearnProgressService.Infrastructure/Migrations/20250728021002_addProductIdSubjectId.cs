using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.LearnProgressService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearnProgressService.Infrastructure.Migrations
{
    public partial class addProductIdSubjectId : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "ProductId",
                    table: GetShardingTableName<LearnProgress>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }


            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.AddColumn<Guid>(
                    name: "SubjectId",
                    table: GetShardingTableName<LearnProgress>(),
                    type: "char(36)",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                    collation: "ascii_general_ci");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.DropColumn(
                    name: "ProductId",
                    table: GetShardingTableName<LearnProgress>());
            }


            if (IsCreateShardingTable<LearnProgress>())
            {
                migrationBuilder.DropColumn(
                    name: "SubjectId",
                    table: GetShardingTableName<LearnProgress>());
            }

        }
    }
}
