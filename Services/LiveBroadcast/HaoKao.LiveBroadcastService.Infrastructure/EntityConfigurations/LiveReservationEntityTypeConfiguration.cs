using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class LiveReservationEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LiveReservation>
{
    public override void Configure(EntityTypeBuilder<LiveReservation> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LiveReservation, Guid>(builder);

        builder.Property(x => x.Phone)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("手机号");

        builder.Property(x => x.OpenId)
               .HasColumnType("varchar")
               .HasMaxLength(255)
               .HasComment("OpenId");

        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)");

        builder.HasIndex(x => new {x.CreatorId, x.Phone,x.OpenId, x.ProductId,x.LiveVideoId, x.TenantId, });
    }
}