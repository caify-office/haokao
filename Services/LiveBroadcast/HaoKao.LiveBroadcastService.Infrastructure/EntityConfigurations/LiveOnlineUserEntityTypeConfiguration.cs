using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class LiveOnlineUserEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LiveOnlineUser>
{
    public override void Configure(EntityTypeBuilder<LiveOnlineUser> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LiveOnlineUser, Guid>(builder);

        builder.Property(x => x.LiveId).HasComment("直播间Id");
        builder.Property(x => x.Phone).HasComment("手机号");
        builder.Property(x => x.IsOnline).HasComment("是否在线");
        builder.Property(x => x.OnlineDuration).HasComment("累计在线时长");
        builder.Property(x => x.CreatorId).HasComment("用户Id");
        builder.Property(x => x.CreatorName).HasComment("用户名称");
        builder.Property(x => x.CreateTime).HasComment("首次上线时间");
        builder.Property(x => x.LastOnlineTime).HasComment("最后上线时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");
        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)");
        builder.HasIndex(x => new { x.LiveId, x.CreatorId, x.TenantId, });
    }
}