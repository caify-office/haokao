using HaoKao.SubjectService.Domain.SubjectModule;
using System;

namespace HaoKao.SubjectService.Infrastructure.Mappings;

public class SubjectEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Subject>
{
    public override void Configure(EntityTypeBuilder<Subject> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Subject, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("科目名称");

        builder.Property(x => x.IsCommon)
               .HasColumnType("int")
               .HasComment("是否是专业科目");

        builder.Property(x => x.Sort)
               .HasColumnType("int")
               .HasComment("排序");

        builder.Property(x => x.TrialSubjectId)
               .HasColumnType("varchar(50)")
               .HasComment("命审题科目Id");

        builder.HasIndex(x => new { x.Name, x.TenantId });
    }
}