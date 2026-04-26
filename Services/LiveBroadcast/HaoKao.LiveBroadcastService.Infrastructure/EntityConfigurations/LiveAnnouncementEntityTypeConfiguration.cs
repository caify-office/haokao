using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class LiveAnnouncementEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LiveAnnouncement>
{
    public override void Configure(EntityTypeBuilder<LiveAnnouncement> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LiveAnnouncement, Guid>(builder);
        builder.Property(x => x.Title)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("标题");

        builder.Property(x => x.Content)
               .HasColumnType("text")
               .HasComment("内容");
    }
}