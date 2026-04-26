

using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Infrastructure.EntityConfigurations;

public class DrawPrizeEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<DrawPrize>
{
    public override void Configure(EntityTypeBuilder<DrawPrize> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<DrawPrize, Guid>(builder);

        builder.Property(x => x.Title)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("名称");

        builder.Property(x => x.BackgroundImageUrl)
            .HasColumnType("varchar")
            .HasMaxLength(1000)
            .HasComment("活动背景图");

        builder.Property(x => x.StartTime)
            .HasColumnType("datetime")
            .HasComment("开始时间");

        builder.Property(x => x.EndTime)
            .HasColumnType("datetime")
            .HasComment("结束时间");

        builder.Property(x => x.Desc)
            .HasColumnType("MEDIUMTEXT")
            .HasComment("说明");


        builder.HasMany(x=>x.Prizes)
            .WithOne(x=>x.DrawPrize)
            .HasForeignKey(x=>x.DrawPrizeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.DrawPrizeRecords)
          .WithOne(x => x.DrawPrize)
          .HasForeignKey(x => x.DrawPrizeId)
          .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.DrawPrizeType);

    }
}