using HaoKao.CourseService.Domain.CourseChapterModule;

namespace HaoKao.CourseService.Infrastructure.Mappings;

public class CourseChapterEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<CourseChapter>
{
    public override void Configure(EntityTypeBuilder<CourseChapter> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<CourseChapter, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("名称");

        builder.HasIndex(x => new { x.CourseId, x.TenantId, });
    }
}