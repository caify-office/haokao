using HaoKao.CampaignService.Domain.Entities;

namespace HaoKao.CampaignService.Infrastructure.Mappings;

public class GiftBagReceiveLogConfiguration : GirvsAbstractEntityTypeConfiguration<GiftBagReceiveLog>
{
    public override void Configure(EntityTypeBuilder<GiftBagReceiveLog> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<GiftBagReceiveLog, Guid>(builder);

        builder.Property(x => x.GiftBagId).HasComment("礼包Id");
        builder.Property(x => x.CampaignName).HasMaxLength(100).IsRequired().HasComment("活动名称");
        builder.Property(x => x.GiftType).HasComment("礼包类型");
        builder.Property(x => x.ProductId).HasComment("产品Id");
        builder.Property(x => x.ProductName).HasMaxLength(100).IsRequired().HasComment("产品名称");
        builder.Property(x => x.ReceiverId).HasComment("领取人Id");
        builder.Property(x => x.ReceiverName).HasMaxLength(50).IsRequired().HasComment("领取人名称");
        builder.Property(x => x.ReceiveTime).HasComment("领取时间");

        builder.HasOne<GiftBag>().WithMany(x => x.GiftBagReceiveLogs).HasForeignKey(x => x.GiftBagId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.ReceiverName, x.ReceiveTime });
        builder.HasIndex(x => new { x.ReceiverId, x.GiftBagId });
    }
}