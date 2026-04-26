using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DataDictionaryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DataDictionaryService.Infrastructure.Migrations
{
    public partial class addArticleColumn : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08dbd42f-c05d-439f-8674-281eb54e8569"), "WZ0151", "文章栏目", "顶级节点", "文章栏目", null, 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08dbd42f-cdd2-4c93-8c91-78beba76b644"), "KS3069", "考试热点", "文章栏目", "文章栏目=>考试热点", new Guid("08dbd42f-c05d-439f-8674-281eb54e8569"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08dbd42f-d0a9-4f0d-8b81-33445afe671e"), "KS8129", "考试动态", "文章栏目", "文章栏目=>考试动态", new Guid("08dbd42f-c05d-439f-8674-281eb54e8569"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbd42f-cdd2-4c93-8c91-78beba76b644"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbd42f-d0a9-4f0d-8b81-33445afe671e"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbd42f-c05d-439f-8674-281eb54e8569"));
            }

        }
    }
}
