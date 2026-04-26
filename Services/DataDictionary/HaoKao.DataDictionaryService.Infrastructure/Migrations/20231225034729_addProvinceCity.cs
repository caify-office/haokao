using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DataDictionaryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DataDictionaryService.Infrastructure.Migrations
{
    public partial class addProvinceCity : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.InsertData(
                    table: GetShardingTableName<Dictionaries>(),
                    columns: new[] { "Id", "Code", "Name", "PName", "Path", "Pid", "Sort", "State", "TenantId" },
                    values: new object[] { new Guid("08dc04f7-1abf-4547-818f-acaac9f63747"), "ProvinceCity", "省份城市", "顶级节点", "省份城市", null, 0, 1ul, new Guid("00000000-0000-0000-0000-000000000000") });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<Dictionaries>())
            {
                migrationBuilder.DeleteData(
                    table: GetShardingTableName<Dictionaries>(),
                    keyColumn: "Id",
                    keyValue: new Guid("08dc04f7-1abf-4547-818f-acaac9f63747"));
            }

        }
    }
}
