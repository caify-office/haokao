using HaoKao.AnsweringQuestionService.Domain.Entities;
using HaoKao.AnsweringQuestionService.Infrastructure.EntityConfigurations;

namespace HaoKao.AnsweringQuestionService.Infrastructure;

[GirvsDbConfig("HaoKao_AnsweringQuestion")]
public class AnsweringQuestionDbContext(DbContextOptions<AnsweringQuestionDbContext> options) : GirvsDbContext(options)
{
    public DbSet<AnsweringQuestion> AnsweringQuestions { get; set; }

    public DbSet<AnsweringQuestionReply> AnsweringQuestionReplies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AnsweringQuestionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AnsweringQuestionReplyEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}