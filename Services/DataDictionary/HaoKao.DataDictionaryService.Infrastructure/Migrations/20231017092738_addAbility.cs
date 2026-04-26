using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DataDictionaryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DataDictionaryService.Infrastructure.Migrations
{
    public partial class addAbility : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08dbcee8-a725-4dec-87b1-13d5be8c4cdd"), "NL4892", "能力维度", "顶级节点", "能力维度", null, 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[,]
                    {
                        { new Guid("08dbcee9-1ac2-4a94-8d53-e04da0eee147"), "JS2336", "计算能力", "能力维度", "能力维度=>计算能力", new Guid("08dbcee8-a725-4dec-87b1-13d5be8c4cdd"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbcee9-2047-4f57-809f-2129bedadb2c"), "LX7165", "练习速度", "能力维度", "能力维度=>练习速度", new Guid("08dbcee8-a725-4dec-87b1-13d5be8c4cdd"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbcee9-233f-494b-8293-0e902b5d3489"), "PD9144", "判断能力", "能力维度", "能力维度=>判断能力", new Guid("08dbcee8-a725-4dec-87b1-13d5be8c4cdd"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbcee9-2576-4dd9-8efb-52510251fd14"), "FX6064", "分析能力", "能力维度", "能力维度=>分析能力", new Guid("08dbcee8-a725-4dec-87b1-13d5be8c4cdd"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbcee9-2798-4721-8795-7174a432323f"), "LJ6732", "理解能力", "能力维度", "能力维度=>理解能力", new Guid("08dbcee8-a725-4dec-87b1-13d5be8c4cdd"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbcee9-29c2-4f1a-8fcd-866667b83d77"), "JY4523", "记忆能力", "能力维度", "能力维度=>记忆能力", new Guid("08dbcee8-a725-4dec-87b1-13d5be8c4cdd"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") }
                    });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbcee9-1ac2-4a94-8d53-e04da0eee147"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbcee9-2047-4f57-809f-2129bedadb2c"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbcee9-233f-494b-8293-0e902b5d3489"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbcee9-2576-4dd9-8efb-52510251fd14"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbcee9-2798-4721-8795-7174a432323f"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbcee9-29c2-4f1a-8fcd-866667b83d77"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbcee8-a725-4dec-87b1-13d5be8c4cdd"));
            }

        }
    }
}
