using HaoKao.StudentService.Domain.Entities;

namespace HaoKao.StudentService.Infrastructure.Mappings;

public class StudentParameterConfigEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<StudentParameterConfig>
{
    public override void Configure(EntityTypeBuilder<StudentParameterConfig> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<StudentParameterConfig, Guid>(builder);

        builder.Property(x => x.NickName).HasMaxLength(50).HasComment("昵称");
        builder.Property(x => x.PropertyName).HasMaxLength(50).HasComment("设置值字段名称");
        builder.Property(x => x.PropertyType).HasMaxLength(500).HasComment("设置值类型");
        builder.Property(x => x.PropertyValue).HasMaxLength(500).HasComment("设置值");
        builder.Property(x => x.Desc).HasMaxLength(1000).HasComment("描述");

        builder.HasIndex(x => new { x.UserId, x.PropertyName });
    }
}