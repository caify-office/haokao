using System;
using Girvs.EntityFrameworkCore.Migrations;
using HaoKao.DeepSeekService.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.DeepSeekService.Infrastructure.Migrations
{
    public partial class Initial : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");


            if (IsCreateShardingTable<DeepSeekConfig>())
            {
                migrationBuilder.CreateTable(
                    name: GetShardingTableName<DeepSeekConfig>(),
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                        ApiKey = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Api密钥")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Endpoint = table.Column<string>(type: "longtext", nullable: false, comment: "接口地址")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Model = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "模型")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        FrequencyPenalty = table.Column<decimal>(type: "decimal(6,2)", nullable: true, comment: "重复内容的可能性"),
                        MaxTokens = table.Column<int>(type: "int", nullable: false, comment: "最大Token"),
                        PresencePenalty = table.Column<decimal>(type: "decimal(6,2)", nullable: true, comment: "新主题的可能性"),
                        ResponseFormat = table.Column<string>(type: "json", nullable: true, comment: "输出格式")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Stop = table.Column<string>(type: "json", nullable: true, comment: "停止词集合")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Stream = table.Column<bool>(type: "tinyint(1)", nullable: true, comment: "是否启用流式输出"),
                        StreamOption = table.Column<string>(type: "json", nullable: true, comment: "流式输出选项")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Temperature = table.Column<decimal>(type: "decimal(6,2)", nullable: true, comment: "采样温度"),
                        TopP = table.Column<decimal>(type: "decimal(6,2)", nullable: true, comment: "采样概率"),
                        Logprobs = table.Column<bool>(type: "tinyint(1)", nullable: true, comment: "是否返回所输出 token 的对数概率"),
                        TopLogprobs = table.Column<int>(type: "int", nullable: true, comment: "返回所输出概率 top N 的 token 的对数概率"),
                        SystemPrompt = table.Column<string>(type: "text", nullable: true, comment: "系统提示")
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        TenantId = table.Column<string>(type: "varchar(36)", nullable: false, comment: "租户Id")
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_DeepSeekConfig", x => x.Id);
                    },
                    comment: "DeepSeek参数配置")
                    .Annotation("MySql:CharSet", "utf8mb4");
            }


            if (IsCreateShardingTable<DeepSeekConfig>())
            {
                migrationBuilder.CreateIndex(
                    name: "IX_DeepSeekConfig_TenantId",
                    table: GetShardingTableName<DeepSeekConfig>(),
                    column: "TenantId");
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (IsCreateShardingTable<DeepSeekConfig>())
            {
                migrationBuilder.DropTable(
                    name: GetShardingTableName<DeepSeekConfig>());
            }

        }
    }
}
