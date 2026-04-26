namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class AccessClientSigningAlgorithmEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<AccessClientSigningAlgorithm>
{
    public override void Configure(EntityTypeBuilder<AccessClientSigningAlgorithm> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessClientSigningAlgorithm, Guid>(builder);
        builder.Property(x => x.SigningAlgorithm).HasMaxLength(250).IsRequired();
    }
}