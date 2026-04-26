using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class LiveCommentEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LiveComment>
{
    public override void Configure(EntityTypeBuilder<LiveComment> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LiveComment, Guid>(builder);

        builder.Property(x => x.Phone)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("手机号");

        builder.Property(x => x.Content)
               .HasColumnType("varchar")
               .HasMaxLength(300)
               .HasComment("评价内容");

        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)");

        builder.HasIndex(x => new { x.Phone, x.LiveId, x.TenantId, });
    }
}