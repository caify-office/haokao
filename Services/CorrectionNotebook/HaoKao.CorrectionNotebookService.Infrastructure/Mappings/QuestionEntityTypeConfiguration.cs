using HaoKao.CorrectionNotebookService.Domain.Entities;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Mappings;

public class QuestionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Question>
{
    public override void Configure(EntityTypeBuilder<Question> builder)
    {
        var entityShardingTableParameter = EngineContext.Current.GetEntityShardingTableParameter<Question>();
        builder.ToTable(entityShardingTableParameter.GetCurrentShardingTableName());

        builder.Property(x => x.ExamLevelId).IsRequired().HasComment("考试级别Id");
        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.ImageUrl).HasColumnType("text").IsRequired().HasComment("题目图片地址");
        builder.Property(x => x.Content).HasColumnType("text").IsRequired().HasComment("题目内容(文本)");
        builder.Property(x => x.Answer).HasColumnType("text").IsRequired().HasComment("题目答案");
        builder.Property(x => x.Analysis).HasColumnType("text").IsRequired().HasComment("题目解析");
        builder.Property(x => x.GenerationTimes).IsRequired().HasComment("生成次数");
        builder.Property(x => x.Generatable).IsRequired().HasComment("是否可生成答案和解析");
        builder.Property(x => x.MasteryDegree).IsRequired().HasComment("掌握程度");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("创建人Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");
        builder.Property(x => x.UpdateTime).IsRequired().HasComment("更新时间");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.CreateTime, x.CreatorId, x.MasteryDegree });

        builder.HasMany(x => x.GenerationLogs).WithOne().HasForeignKey(x => x.QuestionId).OnDelete(DeleteBehavior.Cascade);
    }
}