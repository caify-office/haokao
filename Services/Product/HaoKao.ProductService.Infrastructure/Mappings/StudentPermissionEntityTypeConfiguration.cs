using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Infrastructure.Mappings;

public class StudentPermissionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<StudentPermission>
{
    public override void Configure(EntityTypeBuilder<StudentPermission> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<StudentPermission, Guid>(builder);

        builder.Property(x => x.StudentName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("学员昵称(即用户昵称)");

        builder.Property(x => x.OrderNumber)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("对应的订单号");

        builder.Property(x => x.ProductName)
               .HasColumnType("varchar")
               .HasMaxLength(150)
               .HasComment("产品名称");

        builder.Property(x => x.ExpiryTime)
               .HasColumnType("datetime")
               .HasComment("到期时间");

        builder.HasIndex(x => new { x.StudentId, x.ProductId, x.Enable });

        #region 学习情况统计使用索引

        builder.HasIndex(x => new { x.ProductId, x.Enable, x.TenantId });

        builder.HasIndex(x => new { x.StudentId, x.TenantId });

        #endregion
    }
}