using HaoKao.KnowledgePointService.Domain.Entities;
using HaoKao.KnowledgePointService.Infrastructure.Mapping;

namespace HaoKao.KnowledgePointService.Infrastructure;

[GirvsDbConfig("HaoKao_KnowledgePoint_2023")]
public class KnowledgePointDbContext(DbContextOptions<KnowledgePointDbContext> options) : GirvsDbContext(options)
{
    public DbSet<KnowledgePoint> KnowledgePoints { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new KnowledgePointEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}