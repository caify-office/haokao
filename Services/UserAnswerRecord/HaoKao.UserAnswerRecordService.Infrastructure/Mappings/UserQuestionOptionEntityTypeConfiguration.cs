using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Mappings;

public class UserQuestionOptionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<UserQuestionOption>
{
    public override void Configure(EntityTypeBuilder<UserQuestionOption> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<UserQuestionOption, Guid>(builder);

        builder.Property(x => x.OptionId).HasComment("选项Id");
        builder.Property(x => x.OptionContent).HasColumnType("text").HasComment("回答内容");
    }
}