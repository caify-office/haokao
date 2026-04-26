using HaoKao.CourseService.Domain.CourseModule;

namespace HaoKao.CourseService.Infrastructure.Mappings;

public class CourseEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Course>
{
    public override void Configure(EntityTypeBuilder<Course> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Course, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("课程名称");

        builder.Property(x => x.TeacherJson)
               .HasColumnType("varchar")
               .HasMaxLength(2000)
               .HasComment("主讲老师json");


        builder.Property(x => x.Year)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("年份");

        builder.Property(x => x.SubjectName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("科目名称");

        builder.Property(x => x.UpdateTimeDesc)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("预计更新时间");

        builder.Property(x => x.CourseMaterialsPackageUrl)
              .HasColumnType("varchar")
              .HasMaxLength(500)
              .HasComment("课程讲义包url");

        builder.Property(x => x.CourseMaterialsPackageName)
             .HasColumnType("varchar")
             .HasMaxLength(100)
             .HasComment("课程讲义包名称");

        builder.HasIndex(x => new { x.Id, x.Name, x.SubjectId, x.State, x.TenantId, });
    }
}