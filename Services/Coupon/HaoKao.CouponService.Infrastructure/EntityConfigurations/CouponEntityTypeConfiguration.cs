using HaoKao.CouponService.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HaoKao.CouponService.Infrastructure.EntityConfigurations;

public class CouponEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Coupon>
{
    public override void Configure(EntityTypeBuilder<Coupon> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Coupon, Guid>(builder);

        builder.Property(x => x.EndDate)
               .HasColumnType("datetime")
               .HasComment("有效期-开始时间");

        builder.Property(x => x.BeginDate)
               .HasColumnType("datetime")
               .HasComment("有效期-结束时间");

        builder.Property(x => x.CouponCode)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("优惠券卡号");

        builder.Property(x => x.CouponName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("优惠券名称");

        builder.Property(x => x.CouponDesc)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("优惠券说明");

        builder.Property(x => x.PersonName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("助教名称");

        var productIdsConverter = new ValueConverter<List<Guid>, string>(
            v => JsonConvert.SerializeObject(v),
            v => string.IsNullOrEmpty(v) ? new List<Guid>() : JsonConvert.DeserializeObject<List<Guid>>(v)
        );

        builder.Property(x => x.ProductIds)
               .HasColumnType("json")
               .HasConversion(productIdsConverter)
               .HasComment("适用产品:通过产品包-课程逐级筛选，支持多选和反选。不选产品时，该租户下全场产品通用选择产品集合");


        builder.Property(x => x.Amount)
               .HasColumnType("decimal")
               .HasMaxLength(20).HasPrecision(18, 2)
               .HasComment("金额");

        builder.Property(x => x.ThresholdAmount)
               .HasColumnType("decimal")
               .HasMaxLength(20).HasPrecision(18, 2)
               .HasComment("门槛金额");
    }
}