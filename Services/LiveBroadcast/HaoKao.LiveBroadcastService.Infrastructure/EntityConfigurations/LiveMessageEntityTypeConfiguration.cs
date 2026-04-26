using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class LiveMessageEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LiveMessage>
{
    public override void Configure(EntityTypeBuilder<LiveMessage> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LiveMessage, Guid>(builder);

        builder.Property(x => x.LiveId).HasComment("直播间Id");
        builder.Property(x => x.Content).HasMaxLength(500).HasComment("消息内容");
        builder.Property(x => x.LiveMessageType).HasComment("消息类型");
        builder.Property(x => x.CreatorId).HasComment("用户Id");
        builder.Property(x => x.CreatorName).HasComment("用户名称");
        builder.Property(x => x.CreateTime).HasComment("发送时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)");

        builder.HasIndex(x => new { x.LiveId, x.CreatorId, x.TenantId, });
    }
}