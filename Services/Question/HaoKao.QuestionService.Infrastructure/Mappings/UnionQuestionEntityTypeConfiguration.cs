using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Infrastructure.Mappings;

public class UnionQuestionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<UnionQuestion>
{
    public override void Configure(EntityTypeBuilder<UnionQuestion> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<UnionQuestion, Guid>(builder);

        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.SubjectId, x.QuestionCategoryId, x.TenantId });
    }
}