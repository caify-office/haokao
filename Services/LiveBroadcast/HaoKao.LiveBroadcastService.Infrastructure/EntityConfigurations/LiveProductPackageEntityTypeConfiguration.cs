using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class LiveProductPackageEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LiveProductPackage>
{
    public override void Configure(EntityTypeBuilder<LiveProductPackage> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LiveProductPackage, Guid>(builder);

        builder.Property(x => x.ProductPackageName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("产品包名称");
    }
}