using HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;
using Newtonsoft.Json;

namespace HaoKao.ContinuationService.Infrastructure.Mappings;

public class ProductExtensionPolicyEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ProductExtensionPolicy>
{
    public override void Configure(EntityTypeBuilder<ProductExtensionPolicy> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ProductExtensionPolicy, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("策略名称");

        builder.Property(x => x.StartTime)
               .IsRequired()
               .HasComment("申请开始时间");

        builder.Property(x => x.EndTime)
               .IsRequired()
               .HasComment("申请结束时间");

        builder.Property(x => x.ExtensionType)
               .IsRequired()
               .HasComment("延期类型");

        builder.Property(x => x.ExtensionDays)
               .HasComment("延长天数"); // 可空

        builder.Property(x => x.ExpiryDate)
               .HasComment("固定过期时间"); // 可空

        builder.Property(x => x.Products)
               .HasColumnType("json")
               .IsRequired()
               .HasComment("产品集合")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => string.IsNullOrEmpty(v) ? new List<PolicyProduct>(0) : JsonConvert.DeserializeObject<List<PolicyProduct>>(v)
               );

        builder.Property(x => x.IsEnable)
               .IsRequired()
               .HasComment("是否启用");

        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");
        builder.Property(x => x.IsDelete).HasComment("是否删除标识");

        builder.HasIndex(x => new { x.Name, x.IsEnable, x.TenantId });
    }
}