using HaoKao.LearningPlanService.Domain.Entities;
using HaoKao.LearningPlanService.Infrastructure.EntityConfigurations;

namespace HaoKao.LearningPlanService.Infrastructure;

[GirvsDbConfig("HaoKao_LearningPlan")]
public class LearningPlanDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<LearningPlan> LearningPlans { get; set; }
    public DbSet<LearningTask> LearningTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LearningPlanEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LearningTaskEntityTypeConfiguration());
    }
}