using HaoKao.CourseService.Domain.CourseMaterialsModule;

namespace HaoKao.CourseService.Infrastructure.Mappings;

public class CourseMaterialsEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<CourseMaterials>
{
    public override void Configure(EntityTypeBuilder<CourseMaterials> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<CourseMaterials, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("讲义名称");

        builder.Property(x => x.FileUrl)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("讲义地址");

        builder.HasIndex(x => new { x.CourseChapterId, x.KnowledgePointId, x.TenantId, });
    }
}