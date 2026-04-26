using HaoKao.CouponService.Domain.Models;
using System;

namespace HaoKao.CouponService.Infrastructure.EntityConfigurations;

public class MarketingPersonnelEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<MarketingPersonnel>
{
    public override void Configure(EntityTypeBuilder<MarketingPersonnel> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<MarketingPersonnel, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("姓名");

        builder.Property(x => x.TelPhone)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("手机号码");
    }
}