using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DataDictionaryService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DataDictionaryService.Infrastructure.Migrations;

public partial class InitDatabase : GirvsMigration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");


        if (IsCreateShardingTable<Dictionaries>())
        {
            migrationBuilder.CreateTable(
                    name: GetShardingTableName<Dictionaries>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        Sort = table.Column<int>(type: "int", nullable: false),
                        Code = table.Column<string>(type: "varchar(30)", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Name = table.Column<string>(type: "varchar(225)", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Pid = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                        PName = table.Column<string>(type: "varchar(20)", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Path = table.Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        State = table.Column<ulong>(type: "bit", nullable: true),
                        TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Dictionaries", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<Dictionaries>("FK_Dictionaries_Dictionaries_Pid"),
                            column: x => x.Pid,
                            principalTable: GetShardingTableName<Dictionaries>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    })
                .Annotation("MySql:CharSet", "utf8mb4");
        }


        if (IsCreateShardingTable<Dictionaries>())
        {
            migrationBuilder.CreateIndex(
                name: "IX_Dictionaries_Code_Name_TenantId",
                table: GetShardingTableName<Dictionaries>(),
                columns: new[] { "Code", "Name", "TenantId" },
                unique: true);
        }


        if (IsCreateShardingTable<Dictionaries>())
        {
            migrationBuilder.CreateIndex(
                name: "IX_Dictionaries_Name",
                table: GetShardingTableName<Dictionaries>(),
                column: "Name");
        }


        if (IsCreateShardingTable<Dictionaries>())
        {
            migrationBuilder.CreateIndex(
                name: "IX_Dictionaries_Pid",
                table: GetShardingTableName<Dictionaries>(),
                column: "Pid");
        }


        if (IsCreateShardingTable<Dictionaries>())
        {
            migrationBuilder.CreateIndex(
                name: "IX_Dictionaries_State",
                table: GetShardingTableName<Dictionaries>(),
                column: "State");
        }

    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        if (IsCreateShardingTable<Dictionaries>())
        {
            migrationBuilder.DropTable(
                name: GetShardingTableName<Dictionaries>());
        }

    }
}