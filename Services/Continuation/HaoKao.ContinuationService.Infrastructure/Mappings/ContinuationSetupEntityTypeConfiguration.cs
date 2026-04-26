using HaoKao.ContinuationService.Domain.ContinuationSetupModule;
using Newtonsoft.Json;

namespace HaoKao.ContinuationService.Infrastructure.Mappings;

public class ContinuationSetupEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ContinuationSetup>
{
    public override void Configure(EntityTypeBuilder<ContinuationSetup> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ContinuationSetup, Guid>(builder);

        builder.Property(x => x.StartTime).HasComment("续读申请开始时间");
        builder.Property(x => x.EndTime).HasComment("续读申请结束时间");
        builder.Property(x => x.ExpiryTime).HasComment("续读后的到期时间");
        builder.Property(x => x.Enable).HasComment("是否启用");
        builder.Property(x => x.Products)
               .HasColumnType("json")
               .IsRequired()
               .HasComment("产品集合")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => string.IsNullOrEmpty(v) ? new(0) : JsonConvert.DeserializeObject<List<SetupProduct>>(v)
               );

        builder.Property(x => x.TenantId).HasComment("租户Id");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.IsDelete).HasComment("是否删除标识");
    }
}