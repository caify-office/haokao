using HaoKao.CampaignService.Domain.Entities;

namespace HaoKao.CampaignService.Infrastructure.Mappings;

public class GiftBagConfiguration : GirvsAbstractEntityTypeConfiguration<GiftBag>
{
    public override void Configure(EntityTypeBuilder<GiftBag> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<GiftBag, Guid>(builder);

        builder.Property(x => x.CampaignName).HasMaxLength(100).IsRequired().HasComment("活动名称");
        builder.Property(x => x.GiftType).HasComment("礼包类型");
        builder.Property(x => x.ProductId).HasComment("产品Id");
        builder.Property(x => x.ProductName).HasMaxLength(100).IsRequired().HasComment("产品名称");
        builder.Property(x => x.StartTime).HasComment("开始时间");
        builder.Property(x => x.EndTime).HasComment("结束时间");
        builder.Property(x => x.IsPublished).HasComment("是否发布");
        builder.Property(x => x.Sort).HasComment("排序");
        builder.Property(x => x.ReceiveCount).HasComment("领取数量");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.Property(x => x.WebSiteImageSet).IsRequired().HasComment("PC网站图片")
               .HasColumnType("json").HasConversion(CreateJsonConverter<GiftBagImageSet>());
        builder.Property(x => x.WeChatMiniProgramImageSet).IsRequired().HasComment("微信小程序图片")
               .HasColumnType("json").HasConversion(CreateJsonConverter<GiftBagImageSet>());
        builder.Property(x => x.ReceiveRules).IsRequired().HasComment("领取规则")
               .HasColumnType("json").HasConversion(CreateJsonConverter<IReadOnlyList<Guid>>());

        builder.HasIndex(x => new { x.CampaignName, x.GiftType });
        builder.HasIndex(x => new { x.StartTime, x.EndTime, x.IsPublished });
    }

    private static ValueConverter<T, string> CreateJsonConverter<T>()
    {
        var jsonSerializerOptions = EngineContext.Current.Resolve<IOptions<JsonSerializerOptions>>();
        return new ValueConverter<T, string>(
            v => JsonSerializer.Serialize(v, jsonSerializerOptions.Value),
            v => !string.IsNullOrEmpty(v)
                ? JsonSerializer.Deserialize<T>(v, jsonSerializerOptions.Value)
                : default
        );
    }
}