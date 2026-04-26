using HaoKao.LecturerService.Domain.Entities;

namespace HaoKao.LecturerService.Infrastructure.EntityConfigurations;

public class LecturerEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Lecturer>
{
    public override void Configure(EntityTypeBuilder<Lecturer> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Lecturer, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("教师姓名");

        builder.Property(x => x.SubjectIds)
               .HasColumnType("json")
               .HasComment("科目Id数组");

        builder.Property(x => x.SubjectNames)
               .HasColumnType("json")
               .HasComment("科目名称数组");

        builder.Property(x => x.Desc)
               .HasColumnType("text")
               .HasComment("简介");

        builder.Property(x => x.CourseIntroduction)
               .HasColumnType("text")
               .HasComment("课程介绍");

        builder.Property(x => x.HeadPortraitUrl)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("头像");

        builder.Property(x => x.PhotoUrl)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("形象照");

        builder.Property(x => x.ProductPackageIds)
               .HasColumnType("json")
               .HasComment("关联的产品包Id");


        builder.HasIndex(x => new { x.Name, x.TenantId });
    }
}