using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DataDictionaryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DataDictionaryService.Infrastructure.Migrations
{
    public partial class addReasonContinuationData : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08db94bc-4eda-44e0-8b47-dd2e04233faf"), "XD0266", "续读原因", "顶级节点", "续读原因", null, 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[,]
                    {
                        { new Guid("08db94bc-6fdb-442b-8c29-4cad6d478fdd"), "KS0862", "考试未通过", "续读原因", "续读原因=>考试未通过", new Guid("08db94bc-4eda-44e0-8b47-dd2e04233faf"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db94bc-7408-4ef7-807e-c25a7ee3395e"), "TS9519", "特殊原因缺考", "续读原因", "续读原因=>特殊原因缺考", new Guid("08db94bc-4eda-44e0-8b47-dd2e04233faf"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db94bc-7732-4071-8723-a0c8d3f1173d"), "BD0563", "本地区考试取消", "续读原因", "续读原因=>本地区考试取消", new Guid("08db94bc-4eda-44e0-8b47-dd2e04233faf"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") },
                        { new Guid("08db94bc-7a93-4364-8757-e5e79055a7af"), "JT8748", "其他原因", "续读原因", "续读原因=>其他原因", new Guid("08db94bc-4eda-44e0-8b47-dd2e04233faf"), 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") }
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
                    keyValue: new Guid("08db94bc-6fdb-442b-8c29-4cad6d478fdd"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db94bc-7408-4ef7-807e-c25a7ee3395e"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db94bc-7732-4071-8723-a0c8d3f1173d"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db94bc-7a93-4364-8757-e5e79055a7af"));
            }


            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08db94bc-4eda-44e0-8b47-dd2e04233faf"));
            }

        }
    }
}
