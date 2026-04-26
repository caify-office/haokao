
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Mappings;

public class UnionAnswerRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<UnionAnswerRecord>
{
    public override void Configure(EntityTypeBuilder<UnionAnswerRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<UnionAnswerRecord, Guid>(builder);

        builder.HasMany<UnionAnswerQuestion>().WithOne().HasForeignKey(x => x.UnionAnswerRecordId);

        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.SubjectId, x.QuestionCategoryId, x.CreatorId, x.TenantId });
    }
}

public class UnionAnswerQuestionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<UnionAnswerQuestion>
{
    public override void Configure(EntityTypeBuilder<UnionAnswerQuestion> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<UnionAnswerQuestion, Guid>(builder);

        builder.HasIndex(x => new { x.QuestionId, x.TenantId });
    }
}