namespace HaoKao.WebsiteConfigurationService.Infrastructure.EntityConfigurations;

public class WebsiteTemplateEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<WebsiteTemplate>
{
    public override void Configure(EntityTypeBuilder<WebsiteTemplate> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<WebsiteTemplate, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("名称");

        builder.Property(x => x.Content)
               .HasColumnType("mediumtext")
               .HasComment("内容");

        builder.Property(x => x.Desc)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("描述");

        builder.Property(x => x.ColumnName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("所属栏目名称");

        builder.HasIndex(x => new
        {
            x.ColumnId,
            x.TenantId
        });
    }
}