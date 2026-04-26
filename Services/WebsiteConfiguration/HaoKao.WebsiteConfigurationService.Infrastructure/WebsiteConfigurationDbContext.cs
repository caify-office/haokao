namespace HaoKao.WebsiteConfigurationService.Infrastructure;

[GirvsDbConfig("HaoKao_WebsiteConfiguration")]
public class WebsiteConfigurationDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<Column> Columns { get; set; }

    public DbSet<WebsiteTemplate> Templates { get; set; }

    public DbSet<TemplateStyle> TemplateStyles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ColumnEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TemplateStyleEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new WebsiteTemplateEntityTypeConfiguration());
    }
}