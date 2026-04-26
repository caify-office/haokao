using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Mappings;

public class UserAnswerQuestionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<UserAnswerQuestion>
{
    public override void Configure(EntityTypeBuilder<UserAnswerQuestion> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<UserAnswerQuestion, Guid>(builder);

        builder.Property(x => x.QuestionId).HasComment("试题Id");
        builder.Property(x => x.ParentId).HasComment("父题目Id");
        builder.Property(x => x.UserScore).HasPrecision(4, 1).HasComment("用户得分");
        builder.Property(x => x.JudgeResult).HasComment("判题结果");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.QuestionId, x.TenantId });
    }
}