using Girvs.EntityFrameworkCore.EntityConfigurations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HaoKao.OrderService.Infrastructure.EntityConfigurations;

public class PlatformPayerEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<PlatformPayer>
{
    public override void Configure(EntityTypeBuilder<PlatformPayer> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<PlatformPayer, Guid>(builder);

        var otherConverter = new ValueConverter<Dictionary<string, string>, string>(
            v => JsonConvert.SerializeObject(v),
            v => v.IsNullOrEmpty()
                ? new Dictionary<string, string>()
                : JsonConvert.DeserializeObject<Dictionary<string, string>>(v)
        );

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("支付名称");

        builder.Property(x => x.PayerName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("对应支付处理者名称");

        builder.Property(x => x.Config)
               .HasConversion(otherConverter)
               .HasColumnType("text")
               .HasComment("支付相关配置")
               .Metadata.SetValueComparer(
            new ValueComparer<Dictionary<string, string>>(
                (d1, d2) => d1.SequenceEqual(d2),
                d => d.Aggregate(0, (a, kvp) => HashCode.Combine(a, kvp.Key.GetHashCode(), kvp.Value.GetHashCode())),
                d => new Dictionary<string, string>(d)
            )
        );

        builder.Property(x => x.SecurityCode)
               .HasColumnType("varchar")
               .HasMaxLength(32)
               .HasComment("数据安全码");

        builder.Property(x => x.UpdateTime).HasComment("更新时间");
        builder.Property(x => x.UseState).HasDefaultValue(true).HasComment("启用/禁用");
        builder.Property(x => x.IosIsOpen).HasDefaultValue(false).HasComment("Ios是否开启");

        builder.Property(x => x.CreatorName)
           .HasColumnType("varchar")
           .HasMaxLength(50)
           .HasComment("创建者名称");
    }
}