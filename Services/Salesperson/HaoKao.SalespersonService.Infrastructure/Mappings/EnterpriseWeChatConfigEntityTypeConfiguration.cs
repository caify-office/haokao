
using HaoKao.SalespersonService.Domain.Entities;

namespace HaoKao.SalespersonService.Infrastructure.Mappings;

public class EnterpriseWeChatConfigEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<EnterpriseWeChatConfig>
{
    public override void Configure(EntityTypeBuilder<EnterpriseWeChatConfig> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<EnterpriseWeChatConfig, Guid>(builder);

        builder.Property(x => x.CorpId).HasMaxLength(100).IsRequired().HasComment("企业Id");
        builder.Property(x => x.CorpName).HasMaxLength(100).IsRequired().HasComment("企业名称");
        builder.Property(x => x.CorpSecret).HasMaxLength(100).IsRequired().HasComment("应用的凭证密钥");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => x.TenantId);
    }
}