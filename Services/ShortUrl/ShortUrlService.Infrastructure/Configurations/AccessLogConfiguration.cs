using ShortUrlService.Domain.Entities;

namespace ShortUrlService.Infrastructure.Configurations;

public class AccessLogConfiguration : GirvsAbstractEntityTypeConfiguration<AccessLog>
{
    public override void Configure(EntityTypeBuilder<AccessLog> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessLog, long>(builder);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ShortUrlId).IsRequired().HasComment("短链接Id");
        builder.Property(x => x.Ip).IsRequired().HasColumnType("varchar").HasMaxLength(50).HasComment("IP地址");
        builder.Property(x => x.OsType).IsRequired().HasComment("系统类型");
        builder.Property(x => x.BrowserType).IsRequired().HasComment("浏览器类型");
        builder.Property(x => x.UserAgent).IsRequired().HasColumnType("varchar").HasMaxLength(500).HasComment("UserAgent");
        builder.Property(x => x.CreateTime).HasComment("访问时间");

        builder.HasOne<ShortUrl>().WithMany(x => x.AccessLogs).HasForeignKey(x => x.ShortUrlId).OnDelete(DeleteBehavior.Cascade);
    }
}