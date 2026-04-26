using HaoKao.CourseFeatureService.Domain;

namespace HaoKao.CourseFeatureService.Infrastructure;

public class CourseFeatureEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<CourseFeature>
{
    public override void Configure(EntityTypeBuilder<CourseFeature> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<CourseFeature, Guid>(builder);

        builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired().HasComment("服务名称");
        builder.Property(x => x.Content).HasColumnType("varchar").HasMaxLength(500).IsRequired().HasComment("服务内容");
        builder.Property(x => x.IconUrl).HasColumnType("varchar").HasMaxLength(500).IsRequired().HasComment("图标地址");
        builder.Property(x => x.Enable).HasComment("启用/禁用");
        builder.Property(x => x.TenantId).HasComment("租户Id");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.CreatorId).HasComment("创建人");
    }
}