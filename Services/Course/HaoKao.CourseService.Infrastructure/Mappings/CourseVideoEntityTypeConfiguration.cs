using HaoKao.CourseService.Domain.CourseVideoModule;

namespace HaoKao.CourseService.Infrastructure.Mappings;

public class CourseVideoEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<CourseVideo>
{
    public override void Configure(EntityTypeBuilder<CourseVideo> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<CourseVideo, Guid>(builder);

        builder.Property(x => x.KnowledgePointIds)
               .HasColumnType("varchar")
               .HasMaxLength(2000)
               .HasComment("知识点");

        builder.Property(x => x.Suffix)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("后缀");

        builder.Property(x => x.VideoName)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("视频名称");

        builder.Property(x => x.SourceName)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("视频源名称");

        builder.Property(x => x.VideoUrl)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("播放url-冗余");

        builder.Property(x => x.VideoId)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("视频id");

        builder.Property(x => x.QzName)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("前缀名称");

        builder.Property(x => x.Duration)
               .HasPrecision(18, 2)
               .HasComment("时长");

        builder.Property(x => x.DisplayName)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("显示名称");

        builder.Property(x => x.CateName)
               .HasColumnType("varchar")
               .HasMaxLength(100)
               .HasComment("视频分类名称");

        builder.Property(x => x.Tags)
               .HasColumnType("varchar")
               .HasMaxLength(100)
               .HasComment("视频标签");

        builder.HasIndex(x => new { x.CourseChapterId, x.KnowledgePointId, x.TenantId, });
    }
}