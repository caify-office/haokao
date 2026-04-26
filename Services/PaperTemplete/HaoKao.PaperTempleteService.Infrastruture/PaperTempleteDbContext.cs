using HaoKao.PaperTempleteService.Domain.Entities;
using HaoKao.PaperTempleteService.Infrastructure.EntityConfigurations;

namespace HaoKao.PaperTempleteService.Infrastructure;

[GirvsDbConfig("HaoKao_PaperTemplete")]
public class PaperTempleteDbContext(DbContextOptions<PaperTempleteDbContext> options) : GirvsDbContext(options)
{
    public DbSet<PaperTemplete> PaperTempletes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PaperTempleteEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}