using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class LivePlayBackEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LivePlayBack>
{
    public override void Configure(EntityTypeBuilder<LivePlayBack> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LivePlayBack, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("名称");

        builder.Property(x => x.VideoNo)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("视频序号");

        builder.Property(x => x.Duration)
               .HasPrecision(18, 2)
               .HasColumnType("decimal(18,2)")
               .HasComment("视频时长");
    }
}