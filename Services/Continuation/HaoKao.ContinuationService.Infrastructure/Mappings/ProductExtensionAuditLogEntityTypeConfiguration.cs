using HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

namespace HaoKao.ContinuationService.Infrastructure.Mappings;

public class ProductExtensionAuditLogEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ProductExtensionAuditLog> // Keep this base for consistency in the Mappings folder organization.
{
    public override void Configure(EntityTypeBuilder<ProductExtensionAuditLog> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ProductExtensionAuditLog, Guid>(builder);

        builder.Property(x => x.RequestId)
               .IsRequired()
               .HasComment("关联的申请Id");

        builder.Property(x => x.NewState)
               .IsRequired()
               .HasComment("变更后的状态");

        builder.Property(x => x.Remark)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("审核意见/备注");

        builder.Property(x => x.CreateTime)
               .IsRequired()
               .HasComment("操作时间");

        builder.Property(x => x.CreatorId)
               .IsRequired()
               .HasComment("操作人Id");

        builder.Property(x => x.CreatorName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("操作人名称");

        builder.HasIndex(x => x.RequestId);
    }
}