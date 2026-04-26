namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class AccessClientIdPRestrictionEntityTypeConfiguration :  GirvsAbstractEntityTypeConfiguration<AccessClientIdPRestriction>
{
    public override void Configure(EntityTypeBuilder<AccessClientIdPRestriction> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessClientIdPRestriction,Guid>(builder);
        builder.Property(x => x.Provider).HasMaxLength(200).IsRequired();
    }
}