namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class DomainProxyEntityTypeConfiguration: GirvsAbstractEntityTypeConfiguration<DomainProxy>
{
    public override void Configure(EntityTypeBuilder<DomainProxy> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<DomainProxy,Guid>(builder);
        builder.Property(x => x.Domain).HasMaxLength(300).IsRequired();
        builder.Property(x => x.TenantName).HasMaxLength(500).IsRequired();
    }
}