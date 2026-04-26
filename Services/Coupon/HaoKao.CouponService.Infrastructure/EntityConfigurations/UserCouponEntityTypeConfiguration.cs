using HaoKao.CouponService.Domain.Models;
using System;

namespace HaoKao.CouponService.Infrastructure.EntityConfigurations;

public class UserCouponEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<UserCoupon>
{
    public override void Configure(EntityTypeBuilder<UserCoupon> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<UserCoupon, Guid>(builder);

        builder.Property(x => x.ProductName)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("产品名称");


        builder.Property(x => x.NickName)
               .HasColumnType("varchar")
               .HasMaxLength(200)
               .HasComment("昵称");


        builder.Property(x => x.TelPhone)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("手机号码");

        builder.Property(x => x.FactAmount)
               .HasColumnType("decimal")
               .HasMaxLength(20).HasPrecision(18, 2)
               .HasComment("实际支付金额");
        builder.Property(x => x.Amount)
               .HasColumnType("decimal")
               .HasMaxLength(20).HasPrecision(18, 2)
               .HasComment("订单金额");

        builder.Property(x => x.OpenId)
               .HasColumnType("varchar")
               .HasMaxLength(255)
               .HasComment("OpenId");

        builder.Property(x => x.Notified).HasDefaultValue(false).HasComment("是否已通知");
        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)");
    }
}