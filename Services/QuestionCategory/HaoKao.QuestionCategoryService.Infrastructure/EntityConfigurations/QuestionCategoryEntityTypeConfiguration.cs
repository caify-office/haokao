using HaoKao.QuestionCategoryService.Domain.Entities;
using HaoKao.QuestionCategoryService.Domain.Enums;
using System;

namespace HaoKao.QuestionCategoryService.Infrastructure.EntityConfigurations;

public class QuestionCategoryEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<QuestionCategory>
{
    public override void Configure(EntityTypeBuilder<QuestionCategory> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<QuestionCategory, Guid>(builder);

        builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(50).HasComment("类名名称");
        builder.Property(x => x.Code).HasColumnType("varchar").HasMaxLength(50).HasComment("类别代码");
        builder.Property(x => x.AdaptPlace).HasComment("适应场景");
        builder.Property(x => x.DisplayCondition).HasDefaultValue(DisplayConditionEnum.AlwaysShow).HasComment("显示条件");
        builder.Property(x => x.ProductPackageId).HasDefaultValue(Guid.Empty).HasComment("产品包Id(购买跳转对象)");
        builder.Property(x => x.ProductPackageType).HasComment("产品包类型");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.AdaptPlace, x.Name, x.TenantId });
    }
}