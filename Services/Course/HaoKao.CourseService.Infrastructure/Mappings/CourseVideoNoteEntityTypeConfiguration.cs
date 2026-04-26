using HaoKao.CourseService.Domain.CourseVideoNoteModule;

namespace HaoKao.CourseService.Infrastructure.Mappings;

public class CourseVideoNoteEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<CourseVideoNote>
{
    public override void Configure(EntityTypeBuilder<CourseVideoNote> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<CourseVideoNote, Guid>(builder);

        builder.Property(x => x.VideoId)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("视频id");

        builder.Property(x => x.TimeNode)
               .HasPrecision(18, 2)
               .HasComment("视频时间节点");

        builder.Property(x => x.NoteContent)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("笔记内容");

        builder.Property(x => x.CreatorName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("创建者名称");

        builder.HasIndex(x => new { x.VideoId, x.CreatorId, });
    }
}