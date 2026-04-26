using HaoKao.CourseRatingService.Domain.Entities;
using HaoKao.CourseRatingService.Infrastructure.Mappings;

namespace HaoKao.CourseRatingService.Infrastructure;

[GirvsDbConfig("HaoKao_CourseRating")]
public class CourseRatingRecordDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<CourseRating> CourseRatings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseRatingEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}