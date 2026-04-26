using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Infrastructure.Mappings;

namespace HaoKao.CorrectionNotebookService.Infrastructure;

[GirvsDbConfig("CorrectionNotebook")]
public class CorrectionNotebookDbContext(DbContextOptions<CorrectionNotebookDbContext> options) : GirvsDbContext(options)
{
    public DbSet<ExamLevel> ExamLevels { get; set; }

    public DbSet<Subject> Subjects { get; set; }

    public DbSet<SubjectSort> SubjectSorts { get; set; }

    public DbSet<Question> Questions { get; set; }

    public DbSet<QuestionTag> QuestionTags { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<GenerationLog> GenerationLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ExamLevelEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SubjectEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SubjectSortEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionTagEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TagEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new GenerationLogEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}