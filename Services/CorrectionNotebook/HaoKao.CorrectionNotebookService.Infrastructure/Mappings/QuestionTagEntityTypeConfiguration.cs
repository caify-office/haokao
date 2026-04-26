using HaoKao.CorrectionNotebookService.Domain.Entities;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Mappings;

public class QuestionTagEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<QuestionTag>
{
    public override void Configure(EntityTypeBuilder<QuestionTag> builder)
    {
        var entityShardingTableParameter = EngineContext.Current.GetEntityShardingTableParameter<QuestionTag>();
        builder.ToTable(entityShardingTableParameter.GetCurrentShardingTableName());

        builder.HasKey(x => new { x.QuestionId, x.TagId });

        builder.HasOne<Question>().WithMany(x => x.Tags).HasForeignKey(x => x.QuestionId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Tag).WithMany().HasForeignKey(x => x.TagId).OnDelete(DeleteBehavior.Cascade);
    }
}