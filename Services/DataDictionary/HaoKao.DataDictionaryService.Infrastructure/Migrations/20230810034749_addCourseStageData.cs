using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DataDictionaryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DataDictionaryService.Infrastructure.Migrations
{
    public partial class addCourseStageData : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08db994f-e2e4-40d5-8ed4-18786bbf6e85"), "KC9613", "课程阶段", "顶级节点", "课程阶段", null, 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[,]
                    {
                        { new Guid("08db994f-eaed-458a-81c0-a61cc0a094cb"), "JC1502", "基础阶段", "课程阶段", "课程阶段=>基础阶段", new Guid("08db994f-e2e4-40d5-8ed4-18786bbf6e85"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db994f-eef1-4e74-8f38-828eccdb603e"), "JH1476", "强化阶段", "课程阶段", "课程阶段=>强化阶段", new Guid("08db994f-e2e4-40d5-8ed4-18786bbf6e85"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db994f-f5cb-47fa-861a-80a53511aba8"), "CC5084", "冲刺阶段", "课程阶段", "课程阶段=>冲刺阶段", new Guid("08db994f-e2e4-40d5-8ed4-18786bbf6e85"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db994f-fecf-42b8-87d5-41d7a0da7ac7"), "PS6299", "评审阶段", "课程阶段", "课程阶段=>评审阶段", new Guid("08db994f-e2e4-40d5-8ed4-18786bbf6e85"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9950-037a-4b9d-86c4-472c075235d9"), "JT29225", "其它", "课程阶段", "课程阶段=>其它", new Guid("08db994f-e2e4-40d5-8ed4-18786bbf6e85"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") }
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
                    keyValue: new Guid("08db994f-eaed-458a-81c0-a61cc0a094cb"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db994f-eef1-4e74-8f38-828eccdb603e"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db994f-f5cb-47fa-861a-80a53511aba8"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db994f-fecf-42b8-87d5-41d7a0da7ac7"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9950-037a-4b9d-86c4-472c075235d9"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db994f-e2e4-40d5-8ed4-18786bbf6e85"));
            }

        }
    }
}
