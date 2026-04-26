using HaoKao.CourseFeatureService.Domain;

namespace HaoKao.CourseFeatureService.Infrastructure;

[GirvsDbConfig("HaoKao_CourseFeature")]
public class CourseFeatureDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<CourseFeature> CourseFeatures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseFeatureEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}