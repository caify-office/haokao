using HaoKao.BurialPointService.Domain.Entities;

namespace HaoKao.BurialPointService.Infrastructure.EntityConfigurations;

public class BurialPointEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<BurialPoint>
{
    public override void Configure(EntityTypeBuilder<BurialPoint> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<BurialPoint, Guid>(builder);

        builder.Property(x => x.Name)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("名称");

        builder.HasMany(x => x.BrowseRecords)
            .WithOne()
            .HasForeignKey(x => x.BurialPointId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}