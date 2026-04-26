using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DataDictionaryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DataDictionaryService.Infrastructure.Migrations
{
    public partial class addHotType : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08dbceda-3f6b-4ae2-8493-9466c54511df"), "RD0740", "热点类型", "顶级节点", "热点类型", null, 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[,]
                    {
                        { new Guid("08dbceda-6e0d-4fd5-8eb3-2d281483f10f"), "KS8313", "考试大纲", "热点类型", "热点类型=>考试大纲", new Guid("08dbceda-3f6b-4ae2-8493-9466c54511df"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbceda-7138-4013-8d26-18f6f3a1362b"), "TS4849", "图书发售", "热点类型", "热点类型=>图书发售", new Guid("08dbceda-3f6b-4ae2-8493-9466c54511df"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbceda-73a5-4dae-822d-b43f65f9433d"), "KS3233", "考试报名", "热点类型", "热点类型=>考试报名", new Guid("08dbceda-3f6b-4ae2-8493-9466c54511df"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbceda-75ba-423c-82eb-8ad5c2ed1630"), "XX1600", "学习备考", "热点类型", "热点类型=>学习备考", new Guid("08dbceda-3f6b-4ae2-8493-9466c54511df"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbceda-782f-4fdd-8258-2533aec6727d"), "ZK7113", "准考证", "热点类型", "热点类型=>准考证", new Guid("08dbceda-3f6b-4ae2-8493-9466c54511df"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbceda-a8b7-49a4-8ffa-efb7dcfe7164"), "KS9201", "考试时间", "热点类型", "热点类型=>考试时间", new Guid("08dbceda-3f6b-4ae2-8493-9466c54511df"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbceda-ac1d-4976-8055-ce8a2afdad7b"), "CJ4608", "成绩查询", "热点类型", "热点类型=>成绩查询", new Guid("08dbceda-3f6b-4ae2-8493-9466c54511df"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08dbceda-aefb-4a07-82f9-f2796027cc06"), "ZS0482", "证书领取", "热点类型", "热点类型=>证书领取", new Guid("08dbceda-3f6b-4ae2-8493-9466c54511df"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") }
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
                    keyValue: new Guid("08dbceda-6e0d-4fd5-8eb3-2d281483f10f"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbceda-7138-4013-8d26-18f6f3a1362b"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbceda-73a5-4dae-822d-b43f65f9433d"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbceda-75ba-423c-82eb-8ad5c2ed1630"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbceda-782f-4fdd-8258-2533aec6727d"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbceda-a8b7-49a4-8ffa-efb7dcfe7164"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbceda-ac1d-4976-8055-ce8a2afdad7b"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbceda-aefb-4a07-82f9-f2796027cc06"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dbceda-3f6b-4ae2-8493-9466c54511df"));
            }

        }
    }
}
