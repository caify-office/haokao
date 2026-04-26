using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DataDictionaryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DataDictionaryService.Infrastructure.Migrations
{
    public partial class addSeedData : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08db395d-aeee-47d5-88eb-8a40315e1aad"), "el", "考试级别", "顶级节点", "考试级别", null, 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08db395d-b5db-4dc5-888c-a73d184bc184"), "JJ9930", "经济师", "考试级别", "考试级别=>经济师", new Guid("08db395d-aeee-47d5-88eb-8a40315e1aad"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08db395d-bc27-4d09-86b5-c05dff7deb30"), "XF9930", "消防师", "考试级别", "考试级别=>消防师", new Guid("08db395d-aeee-47d5-88eb-8a40315e1aad"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08db395d-c06c-4875-86da-987b25dba9db"), "KJ9930", "会计师", "考试级别", "考试级别=>会计师", new Guid("08db395d-aeee-47d5-88eb-8a40315e1aad"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db395d-b5db-4dc5-888c-a73d184bc184"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db395d-bc27-4d09-86b5-c05dff7deb30"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db395d-c06c-4875-86da-987b25dba9db"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db395d-aeee-47d5-88eb-8a40315e1aad"));
            }

        }
    }
}
