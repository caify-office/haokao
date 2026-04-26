using HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;
using Newtonsoft.Json;

namespace HaoKao.ContinuationService.Infrastructure.Mappings;

public class ProductExtensionRequestEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ProductExtensionRequest>
{
    public override void Configure(EntityTypeBuilder<ProductExtensionRequest> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ProductExtensionRequest, Guid>(builder);

        // 配置一对多关系：一个请求可以有多个审核日志
        builder.HasMany(r => r.AuditLogs)
               .WithOne() // AuditLog 没有独立的外键导航属性指向 Request，但 Request 有集合
               .HasForeignKey(log => log.RequestId)
               .OnDelete(DeleteBehavior.Cascade); // 请求删除时，其日志也删除

        // 配置其他属性
        builder.Property(x => x.PolicyId)
               .IsRequired()
               .HasComment("关联策略Id");
               
        builder.Property(x => x.ProductId)
               .IsRequired()
               .HasComment("产品Id");

        builder.Property(x => x.ProductName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("产品名称");

        builder.Property(x => x.AgreementId)
               .IsRequired()
               .HasComment("协议Id");

        builder.Property(x => x.AgreementName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("协议名称");

        builder.Property(x => x.ExpiryTime)
               .IsRequired()
               .HasComment("产品过期时间");

        builder.Property(x => x.StudentName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("学员姓名");

        builder.Property(x => x.ReasonId)
               .IsRequired()
               .HasComment("续读原因Id");

        builder.Property(x => x.Description)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("详细描述");

        builder.Property(x => x.Evidences)
               .HasColumnType("json")
               .HasComment("相关证明")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => string.IsNullOrEmpty(v) ? new List<string>(0) : JsonConvert.DeserializeObject<List<string>>(v)
               );
        
        builder.Property(x => x.ProductGifts)
               .HasColumnType("json")
               .HasComment("产品的赠品Id集合")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => string.IsNullOrEmpty(v) ? new List<string>(0) : JsonConvert.DeserializeObject<List<string>>(v)
               );

        builder.Property(x => x.AuditState)
               .IsRequired()
               .HasComment("当前审核状态");

        builder.Property(x => x.AuditReason)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("当前审核意见");

        builder.Property(x => x.AuditTime)
               .HasComment("最后审核时间"); // 可空

        builder.Property(x => x.AuditOperatorName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("最后审核人"); // 可空

        builder.Property(x => x.RestOfCount)
               .IsRequired()
               .HasComment("剩余申请次数");
        
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.CreatorId).HasComment("申请人Id");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.ProductId, x.AgreementId, x.PolicyId, x.CreatorId, x.AuditState, x.TenantId }); // 示例索引
    }
}