using HaoKao.LearnProgressService.Domain.Entities;
using System;

namespace HaoKao.LearnProgressService.Infrastructure.EntityConfigurations;

public class LearnProgressEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LearnProgress>
{
    public override void Configure(EntityTypeBuilder<LearnProgress> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LearnProgress, Guid>(builder);

        builder.Property(x => x.Identifier)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("当前视频标识符,");

        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)");

        builder.Property(x => x.Progress)
              .HasComment("本次学习时长");

        builder.Property(x => x.TotalProgress)
             .HasComment("视频总时长");


        builder.Property(x => x.MaxProgress)
             .HasComment("观看视频最大时长");

        builder.HasIndex(x => new { x.VideoId, x.ChapterId, x.CourseId, x.Identifier, x.CreatorId, x.TenantId });
        #region 学习情况使用索引
        builder.HasIndex(x => new 
        {
           x.CreatorId
           ,x.VideoId
           ,x.TenantId
        });
        #endregion

    }
}