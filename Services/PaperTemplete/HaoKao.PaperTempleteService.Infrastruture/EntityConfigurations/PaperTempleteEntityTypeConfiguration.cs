using HaoKao.PaperTempleteService.Domain.Entities;
using System;

namespace HaoKao.PaperTempleteService.Infrastructure.EntityConfigurations;

public class PaperTempleteEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<PaperTemplete>
{
    public override void Configure(EntityTypeBuilder<PaperTemplete> builder)
    {

        OnModelCreatingBaseEntityAndTableKey<PaperTemplete, Guid>(builder);

        builder.Property(x => x.TempleteName)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("试卷模板名称");

        builder.Property(x => x.Remark)
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .HasComment("备注");

        builder.Property(x => x.SuitableSubjects)
            .HasColumnType("text")
            .HasComment("适用科目");

        builder.Property(x => x.TempleteStructDatas)
            .HasColumnType("text")
            .HasComment("模板结构");

        //索引
        builder.HasIndex(x => new { x.TempleteName, x.TenantId });
    }
}