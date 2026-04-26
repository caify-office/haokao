using HaoKao.QuestionService.Domain.QuestionWrongModule;

namespace HaoKao.QuestionService.Infrastructure.Mappings;

public class QuestionWrongEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<QuestionWrong>
{
    public override void Configure(EntityTypeBuilder<QuestionWrong> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<QuestionWrong, Guid>(builder);

        builder.Property(x => x.QuestionTypeId).IsRequired().HasComment("试题类型Id");
        builder.Property(x => x.ParentQuestionId).HasDefaultValue(Guid.Empty).HasComment("父试题Id");
        builder.Property(x => x.ParentQuestionTypeId).HasDefaultValue(Guid.Empty).HasComment("父试题类型Id");
        builder.Property(x => x.Sort).HasDefaultValue(0).HasComment("排序");

        builder.HasIndex(x => new { x.QuestionId, x.CreatorId, x.TenantId, });
        builder.HasIndex(x => new { x.QuestionId, x.CreatorId, x.QuestionTypeId, x.ParentQuestionTypeId, x.TenantId, });
    }
}