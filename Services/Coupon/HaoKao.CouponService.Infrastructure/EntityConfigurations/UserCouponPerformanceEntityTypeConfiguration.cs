using HaoKao.CouponService.Domain.Models;
using System;

namespace HaoKao.CouponService.Infrastructure.EntityConfigurations;

public class UserCouponPerformanceEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<UserCouponPerformance>
{
    public override void Configure(EntityTypeBuilder<UserCouponPerformance> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<UserCouponPerformance, Guid>(builder);

        builder.Property(x => x.OrderNo)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("订单编号");

        builder.Property(x => x.TelPhone)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("手机号码--冗余");

        builder.Property(x => x.NickName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("昵称--冗余");

        builder.Property(x => x.ProductName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("产品名称--冗余");

        builder.Property(x => x.FactAmount)
               .HasColumnType("decimal")
               .HasMaxLength(20).HasPrecision(18, 2)
               .HasComment("实际支付金额--冗余");

        builder.Property(x => x.Amount)
               .HasColumnType("decimal")
               .HasMaxLength(20).HasPrecision(18, 2)
               .HasComment("产品原价--冗余");

        builder.Property(x => x.PayTime)
               .HasColumnType("datetime")
               .HasComment("支付时间--冗余");

        builder.Property(x => x.Remark)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("备注--后台手动添加的默认手动添加");

        builder.Property(x => x.PersonName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("助教实名名称");

        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)");
    }
}