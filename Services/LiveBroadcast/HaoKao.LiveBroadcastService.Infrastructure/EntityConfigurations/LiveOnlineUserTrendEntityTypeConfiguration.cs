using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class LiveOnlineUserTrendEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LiveOnlineUserTrend>
{
    public override void Configure(EntityTypeBuilder<LiveOnlineUserTrend> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LiveOnlineUserTrend, Guid>(builder);

        builder.Property(x => x.LiveId).HasComment("直播Id");
        builder.Property(x => x.Interval).HasComment("统计间隔(分钟)");
        builder.Property(x => x.TotalCount).HasComment("累计在线人数");
        builder.Property(x => x.OnlineCount).HasComment("当前在线人数");
        builder.Property(x => x.CreateTime).HasComment("统计时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.LiveId, x.TenantId, });
    }
}