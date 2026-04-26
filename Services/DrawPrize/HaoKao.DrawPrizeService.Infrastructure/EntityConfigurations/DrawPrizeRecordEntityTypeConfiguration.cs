using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Infrastructure.EntityConfigurations;

public class DrawPrizeRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<DrawPrizeRecord>
{
    public override void Configure(EntityTypeBuilder<DrawPrizeRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<DrawPrizeRecord, Guid>(builder);

        builder.Property(x => x.Phone)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("手机号");

        builder.Property(x => x.PrizeName)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("奖品名称");
        builder.HasIndex(x => x.DrawPrizeId);
    }
}