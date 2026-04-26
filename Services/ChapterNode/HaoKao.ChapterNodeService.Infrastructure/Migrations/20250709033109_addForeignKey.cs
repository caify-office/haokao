using System;
using Confluent.Kafka;
using Girvs.EntityFrameworkCore.Migrations;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;
using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.ChapterNodeService.Infrastructure.Migrations
{
    public partial class addForeignKey : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var tenantId = EngineContext.Current.ClaimManager?.IdentityClaim?.TenantId;
            if (IsCreateShardingTable<KnowledgePoint>())
            {
                migrationBuilder.AlterColumn<Guid>(
                    name: "ChapterNodeId",
                    table: GetShardingTableName<KnowledgePoint>(),
                    type: "char(36)",
                    nullable: false,
                    comment: "章节Id",
                    collation: "ascii_general_ci",
                    oldClrType: typeof(string),
                    oldType: "varchar(36)",
                    oldComment: "章节Id")
                    .OldAnnotation("MySql:CharSet", "utf8mb4");
            }

            
            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_ChapterNode_ParentId",
                    table: GetShardingTableName<ChapterNode>(),
                    column: "ParentId");
            }

           
            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.AddForeignKey(
                    name: CreateFkName("FK_ChapterNode_ChapterNode_ParentId", tenantId),
                    table: GetShardingTableName<ChapterNode>(),
                    column: "ParentId",
                    principalTable: GetShardingTableName<ChapterNode>(),
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            }


            if (IsCreateShardingTable<KnowledgePoint>())
            {
                migrationBuilder.AddForeignKey(
                    name: CreateFkName("FK_KnowledgePoint_ChapterNode_ChapterNodeId", tenantId),
                    table: GetShardingTableName<KnowledgePoint>(),
                    column: "ChapterNodeId",
                    principalTable: GetShardingTableName<ChapterNode>(),
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            }

        }

        private static string CreateFkName(string baseName, string tenantId)
        {
            return "FK_"+$"{baseName}_{tenantId}".ToMd5();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var tenantId = EngineContext.Current.ClaimManager?.IdentityClaim?.TenantId;
            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.DropForeignKey(
                    name: CreateFkName("FK_ChapterNode_ChapterNode_ParentId", tenantId),
                    table: GetShardingTableName<ChapterNode>());
            }


            if (IsCreateShardingTable<KnowledgePoint>())
            {
                migrationBuilder.DropForeignKey(
                    name:CreateFkName("FK_KnowledgePoint_ChapterNode_ChapterNodeId", tenantId),
                    table: GetShardingTableName<KnowledgePoint>());
            }


            if (IsCreateShardingTable<ChapterNode>())
            {
                migrationBuilder.DropIndex(
                    name: "IX_ChapterNode_ParentId",
                    table: GetShardingTableName<ChapterNode>());
            }


            if (IsCreateShardingTable<KnowledgePoint>())
            {
                migrationBuilder.AlterColumn<string>(
                    name: "ChapterNodeId",
                    table: GetShardingTableName<KnowledgePoint>(),
                    type: "varchar(36)",
                    nullable: false,
                    comment: "章节Id",
                    oldClrType: typeof(Guid),
                    oldType: "char(36)",
                    oldComment: "章节Id")
                    .Annotation("MySql:CharSet", "utf8mb4")
                    .OldAnnotation("Relational:Collation", "ascii_general_ci");
            }

        }
    }
}
