using HaoKao.LearnProgressService.Domain.Entities;
using System;

namespace HaoKao.LearnProgressService.Infrastructure.EntityConfigurations;

public class DailyStudyDurationEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<DailyStudyDuration>
{
    public override void Configure(EntityTypeBuilder<DailyStudyDuration> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<DailyStudyDuration, Guid>(builder);
        builder.Property(x=>x.DailyVideoStudyDuration)
            .HasColumnType("decimal(6, 2)")
            .HasComment("今日学习时长");
        builder.HasIndex(x => new { x.ProductId, x.SubjectId, x.CreatorId });

    }
}