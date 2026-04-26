using HaoKao.CourseService.Domain.VideoStorageModule;

namespace HaoKao.CourseService.Infrastructure.Mappings;

public class VideoStorageEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<VideoStorage>
{
    public override void Configure(EntityTypeBuilder<VideoStorage> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<VideoStorage, Guid>(builder);

        builder.Property(x => x.VideoStorageHandlerName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("视频存储器名称");

        builder.Property(x => x.ConfigParameter)
               .HasColumnType("varchar")
               .HasMaxLength(800)
               .HasComment("视频存储器相关的配置");
    }
}