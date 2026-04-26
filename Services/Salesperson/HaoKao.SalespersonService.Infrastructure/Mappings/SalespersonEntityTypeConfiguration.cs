using HaoKao.SalespersonService.Domain.Entities;

namespace HaoKao.SalespersonService.Infrastructure.Mappings;

public class SalespersonEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Salesperson>
{
    public override void Configure(EntityTypeBuilder<Salesperson> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Salesperson, Guid>(builder);

        builder.Property(x => x.RealName).HasMaxLength(100).IsRequired().HasComment("真实姓名");
        builder.Property(x => x.Phone).HasMaxLength(20).IsRequired().HasComment("手机号");
        builder.Property(x => x.EnterpriseWeChatUserId).HasMaxLength(100).HasComment("企业微信用户Id");
        builder.Property(x => x.EnterpriseWeChatUserName).HasMaxLength(100).HasComment("企业微信昵称");
        builder.Property(x => x.EnterpriseWeChatConfigId).HasComment("企业微信配置Id");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.Phone, x.RealName, x.TenantId });
        builder.HasIndex(x => new { x.RealName, x.TenantId });
        builder.HasIndex(x => x.TenantId);

        builder.HasOne(x => x.EnterpriseWeChatConfig)
               .WithMany()
               .HasForeignKey(x => x.EnterpriseWeChatConfigId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}