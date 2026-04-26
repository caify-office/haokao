using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DataDictionaryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DataDictionaryService.Infrastructure.Migrations
{
    public partial class addCompareData : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08db9271-efed-48e6-8f86-6604089a8b6e"), "KC0415", "课程对比", "顶级节点", "课程对比", null, 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[,]
                    {
                        { new Guid("08db9271-fb47-47e0-8794-481bc7a83aac"), "KG6752", "适合人群", "课程对比", "课程对比=>适合人群", new Guid("08db9271-efed-48e6-8f86-6604089a8b6e"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9271-fef4-4d75-8b1d-daf9ac8bb250"), "JX7250", "教学模块", "课程对比", "课程对比=>教学模块", new Guid("08db9271-efed-48e6-8f86-6604089a8b6e"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-02e9-40f2-8627-5de8abd96dbb"), "ZX1641", "助学模块", "课程对比", "课程对比=>助学模块", new Guid("08db9271-efed-48e6-8f86-6604089a8b6e"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-05ea-4d4e-8466-b7ceaaf9e158"), "JX5456", "教学资料", "课程对比", "课程对比=>教学资料", new Guid("08db9271-efed-48e6-8f86-6604089a8b6e"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-0867-41f5-89f5-9a57f4f49012"), "TS9189", "特色服务", "课程对比", "课程对比=>特色服务", new Guid("08db9271-efed-48e6-8f86-6604089a8b6e"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-0aed-4ab8-81dc-ace77bea8494"), "JG69532", "价格", "课程对比", "课程对比=>价格", new Guid("08db9271-efed-48e6-8f86-6604089a8b6e"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") }
                    });
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[,]
                    {
                        { new Guid("08db9272-624c-4b91-8262-b063d4052f84"), "JC9970", "基础阶段", "教学模块", "课程对比=>教学模块=>基础阶段", new Guid("08db9271-fef4-4d75-8b1d-daf9ac8bb250"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-651e-47bd-8b73-658fbc71f2d3"), "JH1096", "强化阶段", "教学模块", "课程对比=>教学模块=>强化阶段", new Guid("08db9271-fef4-4d75-8b1d-daf9ac8bb250"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-6790-4200-80c2-b56431104573"), "CC9246", "冲刺阶段", "教学模块", "课程对比=>教学模块=>冲刺阶段", new Guid("08db9271-fef4-4d75-8b1d-daf9ac8bb250"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-6ac8-4707-83f1-f827f086f932"), "PS3711", "评审阶段", "教学模块", "课程对比=>教学模块=>评审阶段", new Guid("08db9271-fef4-4d75-8b1d-daf9ac8bb250"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-738b-436d-8502-29575be4bd4a"), "BZ6107", "班主任督学", "助学模块", "课程对比=>助学模块=>班主任督学", new Guid("08db9272-02e9-40f2-8627-5de8abd96dbb"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-76e6-4071-890f-1417fcb49981"), "ZX3244", "在线题库", "助学模块", "课程对比=>助学模块=>在线题库", new Guid("08db9272-02e9-40f2-8627-5de8abd96dbb"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-79e9-4541-8976-4b64ed4478f1"), "FZ8704", "仿真模考测评", "助学模块", "课程对比=>助学模块=>仿真模考测评", new Guid("08db9272-02e9-40f2-8627-5de8abd96dbb"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-8141-4584-82d4-f0a14dcba4a4"), "KJ8174", "课件讲义", "教学资料", "课程对比=>教学资料=>课件讲义", new Guid("08db9272-05ea-4d4e-8466-b7ceaaf9e158"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-83b4-462f-8fa7-9b9e1614b90c"), "KS7049", "考试大纲", "教学资料", "课程对比=>教学资料=>考试大纲", new Guid("08db9272-05ea-4d4e-8466-b7ceaaf9e158"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-85fb-4156-82fe-69186401b3d1"), "JT07377", "其他", "教学资料", "课程对比=>教学资料=>其他", new Guid("08db9272-05ea-4d4e-8466-b7ceaaf9e158"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db9272-8abf-4099-8400-b2dcac2be4ca"), "XD2732", "续读协议", "特色服务", "课程对比=>特色服务=>续读协议", new Guid("08db9272-0867-41f5-89f5-9a57f4f49012"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") }
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
                    keyValue: new Guid("08db9271-fb47-47e0-8794-481bc7a83aac"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-0aed-4ab8-81dc-ace77bea8494"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-624c-4b91-8262-b063d4052f84"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-651e-47bd-8b73-658fbc71f2d3"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-6790-4200-80c2-b56431104573"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-6ac8-4707-83f1-f827f086f932"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-738b-436d-8502-29575be4bd4a"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-76e6-4071-890f-1417fcb49981"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-79e9-4541-8976-4b64ed4478f1"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-8141-4584-82d4-f0a14dcba4a4"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-83b4-462f-8fa7-9b9e1614b90c"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-85fb-4156-82fe-69186401b3d1"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-8abf-4099-8400-b2dcac2be4ca"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9271-fef4-4d75-8b1d-daf9ac8bb250"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-02e9-40f2-8627-5de8abd96dbb"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-05ea-4d4e-8466-b7ceaaf9e158"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9272-0867-41f5-89f5-9a57f4f49012"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db9271-efed-48e6-8f86-6604089a8b6e"));
            }

        }
    }
}
