using HaoKao.PaperService.Domain.Entities;
using System;

namespace HaoKao.PaperService.Infrastructure.EntityConfigurations;

public class PaperEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Paper>
{
    public override void Configure(EntityTypeBuilder<Paper> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Paper, Guid>(builder);

        builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(50).HasComment("试卷名称");
        builder.Property(x => x.SubjectName).HasColumnType("varchar").HasMaxLength(50).HasComment("科目名称");
        builder.Property(x => x.CategoryName).HasColumnType("varchar").HasMaxLength(50).HasComment("分类名称");
        builder.Property(x => x.Score).HasPrecision(18, 2).HasComment("及格分数");
        builder.Property(x => x.Sort).IsRequired().HasComment("排序");
        builder.Property(x => x.QuestionCount).HasComment("总题数");
        builder.Property(x => x.TotalScore).HasColumnType("decimal(6,2)").HasComment("总分数");
        builder.Property(x => x.Year).HasDefaultValue(DateTime.Now.Year).HasComment("年份");

        builder.HasIndex(x => new { x.SubjectId, x.CategoryId, x.TenantId });
    }
}