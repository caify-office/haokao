using HaoKao.StudyMaterialService.Domain.Entities;
using HaoKao.StudyMaterialService.Infrastructure.Mappings;

namespace HaoKao.StudyMaterialService.Infrastructure;

[GirvsDbConfig("HaoKao_StudyMaterial")]
public class StudyMaterialDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<StudyMaterial> StudyMaterials { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StudyMaterialEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}