using HaoKao.QuestionCategoryService.Domain.Entities;
using HaoKao.QuestionCategoryService.Infrastructure.EntityConfigurations;

namespace HaoKao.QuestionCategoryService.Infrastructure;

[GirvsDbConfig("HaoKao_QuestionCategory")]
public class QuestionCategoryDbContext(DbContextOptions<QuestionCategoryDbContext> options) : GirvsDbContext(options)
{
    public DbSet<QuestionCategory> QuestionCategory { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new QuestionCategoryEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}