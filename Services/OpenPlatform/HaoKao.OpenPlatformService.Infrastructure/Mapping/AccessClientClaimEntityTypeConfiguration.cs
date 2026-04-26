namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class AccessClientClaimEntityTypeConfiguration: GirvsAbstractEntityTypeConfiguration<AccessClientClaim>
{
    public override void Configure(EntityTypeBuilder<AccessClientClaim> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessClientClaim,Guid>(builder);
        builder.Property(x => x.Type).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Value).HasMaxLength(250).IsRequired();
    }
}