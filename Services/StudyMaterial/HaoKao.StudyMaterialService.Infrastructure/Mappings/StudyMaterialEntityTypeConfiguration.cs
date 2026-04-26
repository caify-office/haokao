using HaoKao.StudyMaterialService.Domain.Entities;

namespace HaoKao.StudyMaterialService.Infrastructure.Mappings;

public class StudyMaterialEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<StudyMaterial>
{
    public override void Configure(EntityTypeBuilder<StudyMaterial> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<StudyMaterial, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("资料名称");

        builder.Property(x => x.Materials)
               .HasColumnType("json")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => string.IsNullOrEmpty(v) ? new List<Material>() : JsonConvert.DeserializeObject<List<Material>>(v)
               )
               .IsRequired()
               .HasComment("资料内容");

        builder.Property(x => x.Subjects)
               .HasColumnType("text")
               .IsRequired()
               .HasComment("科目");

        builder.Property(x => x.Enable).HasComment("启用/禁用");
        builder.Property(x => x.Year).HasComment("年份");
        builder.Property(x => x.TenantId).HasComment("租户Id");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.CreatorId).HasComment("创建人");
    }
}