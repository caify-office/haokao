using HaoKao.CorrectionNotebookService.Domain.Entities;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Mappings;

public class GenerationLogEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<GenerationLog>
{
    public override void Configure(EntityTypeBuilder<GenerationLog> builder)
    {
        var entityShardingTableParameter = EngineContext.Current.GetEntityShardingTableParameter<GenerationLog>();
        builder.ToTable(entityShardingTableParameter.GetCurrentShardingTableName());

        builder.Property(x => x.QuestionId).IsRequired().HasComment("题目Id");
        builder.Property(x => x.Answer).HasColumnType("text").IsRequired().HasComment("题目答案");
        builder.Property(x => x.Analysis).HasColumnType("text").IsRequired().HasComment("题目解析");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("创建者Id");
        builder.Property(x => x.CreatorName).IsRequired().HasMaxLength(50).HasComment("创建者名称");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.QuestionId, x.CreatorId });
    }
}