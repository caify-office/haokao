namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class AccessClientPropertyEntityTypeConfiguration :  GirvsAbstractEntityTypeConfiguration<AccessClientProperty>
{
    public override void Configure(EntityTypeBuilder<AccessClientProperty> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessClientProperty,Guid>(builder);
        builder.Property(x => x.Key).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Value).HasMaxLength(2000).IsRequired();
    }
}