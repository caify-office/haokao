using HaoKao.Common.RemoteModel;
using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Infrastructure.Mappings;

public class AssistantProductPermissionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<AssistantProductPermission>
{
    public override void Configure(EntityTypeBuilder<AssistantProductPermission> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AssistantProductPermission, Guid>(builder);

        builder.Property(x => x.SubjectName)
               .HasColumnType("varchar(50)")
               .HasComment("对应科目名称");

        builder.Property(x => x.CourseStageName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("课程阶段名称");

        builder.Property(x => x.AssistantProductPermissionContents)
               .HasColumnType("json")
               .HasComment("智辅产品权限内容")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => string.IsNullOrEmpty(v) ? new List<AssistantProductPermissionContent>() : JsonConvert.DeserializeObject<ICollection<AssistantProductPermissionContent>>(v)
               );

        #region 学习情况统计使用

        builder.HasIndex(x => new
        {
            x.SubjectId,
            x.TenantId
        });

        #endregion
    }
}