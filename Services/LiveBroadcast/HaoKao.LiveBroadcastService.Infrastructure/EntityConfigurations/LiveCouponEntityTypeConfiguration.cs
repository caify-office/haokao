using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class LiveCouponEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LiveCoupon>
{
    public override void Configure(EntityTypeBuilder<LiveCoupon> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LiveCoupon, Guid>(builder);

        builder.Property(x => x.LiveCouponName)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("优惠卷名称");

        builder.Property(x => x.Amount)
            .HasColumnType("decimal")
            .HasMaxLength(20)
            .HasPrecision(18, 2)
            .HasComment("金额/折扣--合并一个字段  折扣85折显示0.85");

    }
}