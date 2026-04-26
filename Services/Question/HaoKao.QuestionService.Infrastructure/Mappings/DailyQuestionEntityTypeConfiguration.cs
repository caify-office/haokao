using HaoKao.QuestionService.Domain.DailyQuestionModule;

namespace HaoKao.QuestionService.Infrastructure.Mappings;

public class DailyQuestionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<DailyQuestion>
{
    public override void Configure(EntityTypeBuilder<DailyQuestion> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<DailyQuestion, Guid>(builder);

        builder.Property(x => x.SubjectId).HasComment("科目Id");
        builder.Property(x => x.QuestionId).HasComment("试题Id");
        builder.Property(x => x.CreateDate).HasComment("创建日期");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.QuestionId, x.CreateDate, x.TenantId, }).IsUnique();
    }
}