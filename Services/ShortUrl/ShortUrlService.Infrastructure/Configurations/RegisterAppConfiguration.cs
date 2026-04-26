using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using ShortUrlService.Domain.Entities;

namespace ShortUrlService.Infrastructure.Configurations;

public class RegisterAppConfiguration : GirvsAbstractEntityTypeConfiguration<RegisterApp>
{
    public override void Configure(EntityTypeBuilder<RegisterApp> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<RegisterApp, long>(builder);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.AppName).IsRequired().HasMaxLength(50).HasComment("应用名称");
        builder.Property(x => x.AppCode).IsRequired().HasMaxLength(50).HasComment("应用编码");
        builder.Property(x => x.AppSecret).IsRequired().HasMaxLength(512).HasComment("应用密钥");
        builder.Property(x => x.Description).HasDefaultValue(string.Empty).HasMaxLength(200).HasComment("应用描述");
        builder.Property(x => x.IsEnable).HasDefaultValue(true).HasComment("是否启用");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.AppDomains)
               .HasColumnType("json")
               .HasComment("应用域名")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>(0),
                   new ValueComparer<List<string>>(
                       (c1, c2) => (c1 ?? new List<string>(0)).SequenceEqual(c2 ?? new List<string>(0)),
                       c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                       c => c.ToList()
                   )
               );

        builder.HasIndex(x => new { x.AppName, x.AppCode, }).IsUnique();
        builder.HasIndex(x => new { x.AppCode, x.AppSecret, }).IsUnique();
    }
}