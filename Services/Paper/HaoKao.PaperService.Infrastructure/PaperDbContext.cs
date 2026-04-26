using HaoKao.PaperService.Domain.Entities;
using HaoKao.PaperService.Infrastructure.EntityConfigurations;

namespace HaoKao.PaperService.Infrastructure;

[GirvsDbConfig("HaoKao_Paper_2023")]
public class PaperDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<Paper> Papers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PaperEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}