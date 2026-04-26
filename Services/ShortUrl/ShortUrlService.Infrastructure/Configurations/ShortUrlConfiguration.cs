using ShortUrlService.Domain.Entities;

namespace ShortUrlService.Infrastructure.Configurations;

public class ShortUrlConfiguration : GirvsAbstractEntityTypeConfiguration<ShortUrl>
{
    public override void Configure(EntityTypeBuilder<ShortUrl> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ShortUrl, long>(builder);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.ShortKey).IsRequired().HasColumnType("varchar").HasMaxLength(50).HasComment("短链接后缀");
        builder.Property(x => x.OriginUrl).IsRequired().HasColumnType("varchar").HasMaxLength(500).HasComment("原始Url");
        builder.Property(x => x.AccessLimit).IsRequired().HasComment("可访问次数");
        builder.Property(x => x.ExpiredTime).IsRequired().HasComment("过期时间");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.IsDelete).HasDefaultValue(false).HasComment("是否删除标识");

        builder.HasIndex(x => new { x.ShortKey, x.IsDelete });
        builder.HasIndex(x => new { x.RegisterAppId, x.OriginUrl, x.IsDelete, });

        builder.HasOne<RegisterApp>().WithMany(x => x.ShortUrls).HasForeignKey(x => x.RegisterAppId).OnDelete(DeleteBehavior.Cascade);
    }
}