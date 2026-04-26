namespace HaoKao.WebsiteConfigurationService.Infrastructure.EntityConfigurations;

public class ColumnEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Column>
{
    public override void Configure(EntityTypeBuilder<Column> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Column, Guid>(builder);
        builder.Property(x => x.DomainName)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("域名");

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("名称");

        builder.Property(x => x.Alias)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("别名");

        builder.Property(x => x.EnglishName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("英文名");

        builder.Property(x => x.Icon)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("图标");

        builder.Property(x => x.ActiveIcon)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("当前图标");

        builder.HasIndex(x =>
                             new
                             {
                                 x.EnglishName,
                                 x.DomainName,
                                 x.TenantId
                             });
    }
}