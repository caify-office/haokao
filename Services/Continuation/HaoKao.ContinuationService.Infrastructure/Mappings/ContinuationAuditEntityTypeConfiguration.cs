using HaoKao.ContinuationService.Domain.ContinuationAuditModule;
using Newtonsoft.Json;

namespace HaoKao.ContinuationService.Infrastructure.Mappings;

public class ContinuationAuditEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ContinuationAudit>
{
    public override void Configure(EntityTypeBuilder<ContinuationAudit> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ContinuationAudit, Guid>(builder);

        builder.Property(x => x.ProductName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("产品名称");

        builder.Property(x => x.AgreementName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("协议名称");

        builder.Property(x => x.StudentName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("学员姓名");

        builder.Property(x => x.Description)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .IsRequired()
               .HasComment("详细描述");

        builder.Property(x => x.Evidences)
               .HasColumnType("json")
               .IsRequired()
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

        builder.Property(x => x.AuditReason)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .IsRequired()
               .HasComment("不通过原因");

        builder.Property(x => x.RestOfCount)
               .IsRequired()
               .HasComment("剩余申请次数");

        builder.Property(x => x.Reason).HasComment("续读原因");
        builder.Property(x => x.SetupId).HasComment("续读配置Id");
        builder.Property(x => x.ProductId).HasComment("产品Id");
        builder.Property(x => x.AgreementId).HasComment("协议Id");
        builder.Property(x => x.ExpiryTime).HasComment("产品过期时间");
        builder.Property(x => x.AuditState).HasComment("审核状态");
        builder.Property(x => x.TenantId).HasComment("租户Id");
        builder.Property(x => x.CreateTime).HasComment("申请时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.CreatorId).HasComment("申请人");

        builder.HasIndex(x => new { x.ProductId, x.AgreementId, x.SetupId, x.CreatorId, x.TenantId });
    }
}