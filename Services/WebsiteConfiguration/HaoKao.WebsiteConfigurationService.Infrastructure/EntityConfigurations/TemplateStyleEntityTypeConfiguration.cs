namespace HaoKao.WebsiteConfigurationService.Infrastructure.EntityConfigurations;

public class TemplateStyleEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<TemplateStyle>
{
    public override void Configure(EntityTypeBuilder<TemplateStyle> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<TemplateStyle, Guid>(builder);

        builder.Property(x => x.DomainName)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("域名");

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("名称");

        builder.Property(x => x.Path)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("内容");

        builder.HasIndex(x => new
        {
            x.DomainName,
            x.TenantId
        });
    }
}