using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;
using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;
using HaoKao.ChapterNodeService.Infrastructure.ChapterNodeModule;
using HaoKao.ChapterNodeService.Infrastructure.KnowledgePointModule;

namespace HaoKao.ChapterNodeService.Infrastructure;

[GirvsDbConfig("HaoKao_ChapterNode")]
public class ChapterNodeDbContext(DbContextOptions<ChapterNodeDbContext> options) : GirvsDbContext(options)
{
    public DbSet<ChapterNode> ChapterNodes { get; set; }

    public DbSet<KnowledgePoint> KnowledgePoints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChapterNodeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new KnowledgePointEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}