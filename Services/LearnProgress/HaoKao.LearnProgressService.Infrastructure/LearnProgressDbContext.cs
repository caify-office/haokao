using HaoKao.LearnProgressService.Domain.Entities;
using HaoKao.LearnProgressService.Infrastructure.EntityConfigurations;

namespace HaoKao.LearnProgressService.Infrastructure;

[GirvsDbConfig("HaoKao_LearnProgress")]
public class LearnProgressDbContext(DbContextOptions<LearnProgressDbContext> options) : GirvsDbContext(options)
{
    public DbSet<LearnProgress> LearnProgresses { get; set; }
    public DbSet<DailyStudyDuration> DailyStudyDurations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LearnProgressEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DailyStudyDurationEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}