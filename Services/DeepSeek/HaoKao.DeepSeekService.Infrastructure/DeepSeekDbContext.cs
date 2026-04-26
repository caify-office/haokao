using HaoKao.DeepSeekService.Domain;

namespace HaoKao.DeepSeekService.Infrastructure;

[GirvsDbConfig("HaoKao_DeepSeek")]
public class DeepSeekDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<DeepSeekConfig> DeepSeekConfigs { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new DeepSeekConfigEntityTypeConfiguration());
    }
}