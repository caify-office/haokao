using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Seeds;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Mappings;

public class TagEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Tag>
{
    public override void Configure(EntityTypeBuilder<Tag> builder)
    {
        var entityShardingTableParameter = EngineContext.Current.GetEntityShardingTableParameter<Tag>();
        builder.ToTable(entityShardingTableParameter.GetCurrentShardingTableName());

        builder.Property(x => x.Category).HasMaxLength(10).IsRequired().HasComment("标签分类");
        builder.Property(x => x.Name).HasMaxLength(10).IsRequired().HasComment("标签名称");
        builder.Property(x => x.IsBuiltIn).IsRequired().HasComment("是否内置数据");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("创建人Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.CreatorId });

        builder.HasData(TagSeed.Data);
    }
}