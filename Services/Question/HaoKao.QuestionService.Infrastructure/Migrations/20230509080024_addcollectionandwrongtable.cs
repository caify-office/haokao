using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.QuestionService.Domain.QuestionCollectionModule;
using HaoKao.QuestionService.Domain.QuestionModule;
using HaoKao.QuestionService.Domain.QuestionWrongModule;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.QuestionService.Infrastructure.Migrations
{
    public partial class addcollectionandwrongtable : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCollection>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<QuestionCollection>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        QuestionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_QuestionCollection", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<QuestionCollection>("FK_QuestionCollection_Question_QuestionId"),
                            column: x => x.QuestionId,
                            principalTable: GetShardingTableName<Question>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<QuestionWrong>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        QuestionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreatorId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_QuestionWrong", x => x.Id);
                        table.ForeignKey(
                            name: GetShardingForeignKey<QuestionWrong>("FK_QuestionWrong_Question_QuestionId"),
                            column: x => x.QuestionId,
                            principalTable: GetShardingTableName<Question>(),
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<QuestionCollection>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_QuestionCollection_QuestionId_CreatorId_TenantId",
                    table: GetShardingTableName<QuestionCollection>(),
                    columns: new[] { "QuestionId", "CreatorId", "TenantId" });
            }


            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_QuestionWrong_QuestionId_CreatorId_TenantId",
                    table: GetShardingTableName<QuestionWrong>(),
                    columns: new[] { "QuestionId", "CreatorId", "TenantId" });
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<QuestionCollection>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<QuestionCollection>());
            }


            if (IsCreateShardingTable<QuestionWrong>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<QuestionWrong>());
            }

        }
    }
}
