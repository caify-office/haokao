using HaoKao.QuestionService.Domain.DailyQuestionModule;
using HaoKao.QuestionService.Domain.QuestionCollectionModule;
using HaoKao.QuestionService.Domain.QuestionModule;
using HaoKao.QuestionService.Domain.QuestionWrongModule;
using HaoKao.QuestionService.Domain.QuestionWrongPaperMoudle;
using HaoKao.QuestionService.Infrastructure.Mappings;

namespace HaoKao.QuestionService.Infrastructure;

[GirvsDbConfig("HaoKao_Question")]
public class QuestionDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<Question> Questions { get; init; }

    public DbSet<QuestionCollection> QuestionCollections { get; init; }

    public DbSet<QuestionWrong> QuestionWrongs { get; init; }

    public DbSet<QuestionWrongPaper> QuestionWrongPapers { get; init; }

    public DbSet<DailyQuestion> DailyQuestions { get; init; }

    public DbSet<UnionQuestion> UnionQuestions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new QuestionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionCollectionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionWrongEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionWrongPaperEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DailyQuestionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UnionQuestionEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}