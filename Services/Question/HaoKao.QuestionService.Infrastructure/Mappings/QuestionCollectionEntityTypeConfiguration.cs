using HaoKao.QuestionService.Domain.QuestionCollectionModule;

namespace HaoKao.QuestionService.Infrastructure.Mappings;

public class QuestionCollectionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<QuestionCollection>
{
    public override void Configure(EntityTypeBuilder<QuestionCollection> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<QuestionCollection, Guid>(builder);

        builder.HasIndex(x => new { x.QuestionId, x.CreatorId, x.TenantId, });
    }
}