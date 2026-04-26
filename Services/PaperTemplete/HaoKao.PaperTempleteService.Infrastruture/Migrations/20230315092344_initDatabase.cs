using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.PaperTempleteService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.PaperTempleteService.Infrastructure.Migrations
{
    public partial class initDatabase : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<PaperTemplete>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        TempleteName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "试卷模板名称")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Remark = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "备注")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        SuitableSubjects = table.Column<string>(type: "text", nullable: true, comment: "适用科目")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TempleteStructDatas = table.Column<string>(type: "text", nullable: true, comment: "模板结构")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_PaperTemplete", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_PaperTemplete_TempleteName",
                    table: GetShardingTableName<PaperTemplete>(),
                    column: "TempleteName");
            }


            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_PaperTemplete_TenantId",
                    table: GetShardingTableName<PaperTemplete>(),
                    column: "TenantId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<PaperTemplete>());
            }

        }
    }
}
