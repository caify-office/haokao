using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.PaperTempleteService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.PaperTempleteService.Infrastructure.Migrations
{
    public partial class optimizeIndex : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_PaperTemplete_TempleteName",
                    table: GetShardingTableName<PaperTemplete>());
            }


            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_PaperTemplete_TenantId",
                    table: GetShardingTableName<PaperTemplete>());
            }


            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "TempleteName",
                    table: GetShardingTableName<PaperTemplete>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: true,
                    comment: "试卷模板名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldComment: "试卷模板名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Remark",
                    table: GetShardingTableName<PaperTemplete>(),
                    type: "varchar(100)",
                    maxLength: 100,
                    nullable: true,
                    comment: "备注",
                    oldClrType: typeof(string),
                    oldType: "varchar(100)",
                    oldMaxLength: 100,
                    oldComment: "备注")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_PaperTemplete_TempleteName_TenantId",
                    table: GetShardingTableName<PaperTemplete>(),
                    columns: new[] { "TempleteName", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_PaperTemplete_TempleteName_TenantId",
                    table: GetShardingTableName<PaperTemplete>());
            }


            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<PaperTemplete>(),
                    keyColumn: "TempleteName",
                    keyValue: null,
                    column: "TempleteName",
                    value: "");
            }


            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "TempleteName",
                    table: GetShardingTableName<PaperTemplete>(),
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: false,
                    comment: "试卷模板名称",
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50,
                    oldNullable: true,
                    oldComment: "试卷模板名称")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.UpdateData(
                    table: GetShardingTableName<PaperTemplete>(),
                    keyColumn: "Remark",
                    keyValue: null,
                    column: "Remark",
                    value: "");
            }


            if (IsCreateShardingTable<PaperTemplete>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "Remark",
                    table: GetShardingTableName<PaperTemplete>(),
                    type: "varchar(100)",
                    maxLength: 100,
                    nullable: false,
                    comment: "备注",
                    oldClrType: typeof(string),
                    oldType: "varchar(100)",
                    oldMaxLength: 100,
                    oldNullable: true,
                    oldComment: "备注")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
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
    }
}
