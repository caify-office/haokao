using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Seeds;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Mappings;

public class SubjectEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Subject>
{
    public override void Configure(EntityTypeBuilder<Subject> builder)
    {
        var entityShardingTableParameter = EngineContext.Current.GetEntityShardingTableParameter<Subject>();
        builder.ToTable(entityShardingTableParameter.GetCurrentShardingTableName());

        builder.Property(x => x.ExamLevelId).IsRequired().HasComment("考试级别Id");
        builder.Property(x => x.Name).HasMaxLength(20).IsRequired().HasComment("科目名称");
        builder.Property(x => x.Icon).HasColumnType("text").IsRequired().HasComment("科目图标");
        builder.Property(x => x.IsBuiltIn).IsRequired().HasComment("是否内置数据");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("创建人Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.CreatorId, x.IsBuiltIn });

        builder.HasMany(x => x.Questions).WithOne().HasForeignKey(x => x.SubjectId);
        builder.HasMany(x => x.Sorts).WithOne().HasForeignKey(x => x.SubjectId).OnDelete(DeleteBehavior.Cascade);

        builder.HasData(SubjectSeed.Data);
    }
}