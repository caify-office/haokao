using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Infrastructure.Mappings;

namespace HaoKao.UserAnswerRecordService.Infrastructure;

[GirvsDbConfig("HaoKao_UserAnswerRecord")]
public class UserAnswerRecordDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<UserAnswerRecord> UserAnswerRecords { get; init; }

    public DbSet<UserAnswerQuestion> UserAnswerQuestions { get; init; }

    public DbSet<UserQuestionOption> UserQuestionOptions { get; init; }

    public DbSet<UnionAnswerRecord> UnionAnswerRecords { get; init; }

    public DbSet<UnionAnswerQuestion> UnionAnswerQuestions { get; init; }

    public DbSet<AnswerRecord> AnswerRecords { get; init; }

    public DbSet<AnswerQuestion> AnswerQuestions { get; init; }

    public DbSet<ChapterAnswerRecord> ChapterAnswerRecords { get; init; }

    public DbSet<DateAnswerRecord> DateAnswerRecords { get; init; }

    public DbSet<PaperAnswerRecord> PaperAnswerRecords { get; init; }

    public DbSet<ElapsedTimeRecord> ElapsedTimeRecords { get; init; }

    public DbSet<ProductChapterAnswerRecord> ProductChapterAnswerRecords { get; init; }

    public DbSet<ProductPaperAnswerRecord> ProductPaperAnswerRecords { get; init; }

    public DbSet<ProductKnowledgeAnswerRecord> ProductKnowledgeAnswerRecords { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserAnswerRecordEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserAnswerQuestionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserQuestionOptionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UnionAnswerRecordEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UnionAnswerQuestionEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new AnswerRecordEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AnswerQuestionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ChapterAnswerRecordEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DateAnswerRecordEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaperAnswerRecordEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ElapsedTimeRecordEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new ProductChapterAnswerRecordEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductPaperAnswerRecordEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductKnowledgeAnswerRecordEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}