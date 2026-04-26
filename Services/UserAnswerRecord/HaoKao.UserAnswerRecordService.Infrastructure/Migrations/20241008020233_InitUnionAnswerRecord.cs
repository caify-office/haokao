using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Migrations
{
    public partial class InitUnionAnswerRecord : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UnionAnswerRecord>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<UnionAnswerRecord>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        RecordIdentifier = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        QuestionCategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_UnionAnswerRecord", x => x.Id);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UnionAnswerQuestion>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<UnionAnswerQuestion>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        UnionAnswerRecordId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        QuestionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        QuestionTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                        JudgeResult = table.Column<int>(type: "int", nullable: false),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_UnionAnswerQuestion", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<UnionAnswerQuestion>("FK_UnionAnswerQuestion_UnionAnswerRecord_UnionAnswerRecordId"),
                            column: x => x.UnionAnswerRecordId,
                            principalTable: GetShardingTableName<UnionAnswerRecord>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<UnionAnswerQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UnionAnswerQuestion_QuestionId_TenantId",
                    table: GetShardingTableName<UnionAnswerQuestion>(),
                    columns: new[] { "QuestionId", "TenantId" });
            }


            if (IsCreateShardingTable<UnionAnswerQuestion>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UnionAnswerQuestion_UnionAnswerRecordId",
                    table: GetShardingTableName<UnionAnswerQuestion>(),
                    column: "UnionAnswerRecordId");
            }


            if (IsCreateShardingTable<UnionAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UnionAnswerRecord_SubjectId_QuestionCategoryId_CreatorId_Ten~",
                    table: GetShardingTableName<UnionAnswerRecord>(),
                    columns: new[] { "SubjectId", "QuestionCategoryId", "CreatorId", "TenantId" });
            }


            if (IsCreateShardingTable<UnionAnswerRecord>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_UnionAnswerRecord_TenantId",
                    table: GetShardingTableName<UnionAnswerRecord>(),
                    column: "TenantId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<UnionAnswerQuestion>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<UnionAnswerQuestion>());
            }


            if (IsCreateShardingTable<UnionAnswerRecord>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<UnionAnswerRecord>());
            }

        }
    }
}
