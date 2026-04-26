using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Seeds;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Mappings;

public class ExamLevelEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ExamLevel>
{
    public override void Configure(EntityTypeBuilder<ExamLevel> builder)
    {
        var entityShardingTableParameter = EngineContext.Current.GetEntityShardingTableParameter<ExamLevel>();
        builder.ToTable(entityShardingTableParameter.GetCurrentShardingTableName());

        builder.Property(x => x.Name).HasMaxLength(20).IsRequired().HasComment("考试级别名称");
        builder.Property(x => x.IsBuiltIn).IsRequired().HasComment("是否内置数据");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("创建人Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");
        builder.Property(x => x.ParentId).IsRequired().HasComment("父级Id");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.ParentId);
        builder.HasIndex(x => new { x.CreatorId, x.IsBuiltIn });

        builder.HasMany(x => x.Subjects).WithOne().HasForeignKey(x => x.ExamLevelId);

        builder.HasData(ExamLevelSeed.Data);
    }
}