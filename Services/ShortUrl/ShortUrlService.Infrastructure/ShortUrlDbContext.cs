using ShortUrlService.Domain.Entities;
using ShortUrlService.Infrastructure.Configurations;

namespace ShortUrlService.Infrastructure;

[GirvsDbConfig("ShortUrl")]
public class ShortUrlDbContext(DbContextOptions<ShortUrlDbContext> options) : GirvsDbContext(options)
{
    public DbSet<ShortUrl> ShortUrls { get; set; }

    public DbSet<AccessLog> AccessLogs { get; set; }

    public DbSet<RegisterApp> RegisterApps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ShortUrlConfiguration());
        modelBuilder.ApplyConfiguration(new AccessLogConfiguration());
        modelBuilder.ApplyConfiguration(new RegisterAppConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}