using HaoKao.Common.Extensions;
using HaoKao.DeepSeekService.Domain;

namespace HaoKao.DeepSeekService.Infrastructure;

public class DeepSeekConfigEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<DeepSeekConfig>
{
    public override void Configure(EntityTypeBuilder<DeepSeekConfig> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<DeepSeekConfig, Guid>(builder);

        builder.Property(x => x.ApiKey).HasColumnType("varchar").HasMaxLength(50).IsRequired().HasComment("Api密钥");
        builder.Property(x => x.Endpoint).IsRequired().HasComment("接口地址");
        builder.Property(x => x.Model).HasColumnType("varchar").HasMaxLength(50).IsRequired().HasComment("模型");
        builder.Property(x => x.FrequencyPenalty).HasColumnType("decimal(6, 2)").HasComment("重复内容的可能性");
        builder.Property(x => x.MaxTokens).HasComment("最大Token");
        builder.Property(x => x.PresencePenalty).HasColumnType("decimal(6, 2)").HasComment("新主题的可能性");
        builder.Property(x => x.ResponseFormat).HasColumnType("json").HasComment("输出格式")
               .HasConversion(JsonConversion.Create<ResponseFormat>());
        builder.Property(x => x.Stop).HasColumnType("json").HasComment("停止词集合");
        builder.Property(x => x.Stream).HasComment("是否启用流式输出");
        builder.Property(x => x.StreamOption).HasColumnType("json").HasComment("流式输出选项")
               .HasConversion(JsonConversion.Create<StreamOption>());
        builder.Property(x => x.Temperature).HasColumnType("decimal(6, 2)").HasComment("采样温度");
        builder.Property(x => x.TopP).HasColumnType("decimal(6, 2)").HasComment("采样概率");
        builder.Property(x => x.Logprobs).HasComment("是否返回所输出 token 的对数概率");
        builder.Property(x => x.TopLogprobs).HasComment("返回所输出概率 top N 的 token 的对数概率");
        builder.Property(x => x.SystemPrompt).HasColumnType("text").HasComment("系统提示");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => x.TenantId);
    }
}