using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Seeds;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Mappings;

public class SubjectSortEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<SubjectSort>
{
    public override void Configure(EntityTypeBuilder<SubjectSort> builder)
    {
        var entityShardingTableParameter = EngineContext.Current.GetEntityShardingTableParameter<SubjectSort>();
        builder.ToTable(entityShardingTableParameter.GetCurrentShardingTableName());

        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("创建人Id");
        builder.Property(x => x.Priority).IsRequired().HasComment("排序");
        builder.Property(x => x.IsBuiltIn).IsRequired().HasComment("是否内置数据");

        builder.HasIndex(x => new { x.CreatorId, x.SubjectId });

        builder.HasData(SubjectSortSeed.Data);
    }
}