using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Infrastructure.Mappings;

public class ProductPermissionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ProductPermission>
{
    public override void Configure(EntityTypeBuilder<ProductPermission> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ProductPermission, Guid>(builder);

        builder.Property(x => x.PermissionName)
               .HasMaxLength(150)
               .HasComment("产品权限名称");

        builder.Property(x => x.Category)
               .HasMaxLength(150)
               .HasComment("产品权限属性分类");

        builder.Property(x => x.SubjectName)
               .HasMaxLength(150)
               .HasComment("科目名称");

        #region 学习情况统计使用

        builder.HasIndex(x => new
        {
            x.PermissionId,
            x.TenantId
        });

        #endregion
    }
}