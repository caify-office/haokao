using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Infrastructure.Mappings;

public class QuestionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Question>
{
    public override void Configure(EntityTypeBuilder<Question> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Question, Guid>(builder);

        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.SubjectName).HasColumnType("varchar").HasMaxLength(50).IsRequired().HasComment("科目名称");
        builder.Property(x => x.ChapterId).HasComment("章节Id");
        builder.Property(x => x.ChapterName).HasColumnType("varchar").HasMaxLength(100).HasComment("章节名称");
        builder.Property(x => x.SectionId).HasComment("小节Id");
        builder.Property(x => x.SectionName).HasColumnType("varchar").HasMaxLength(100).HasComment("小节名称");
        builder.Property(x => x.KnowledgePointId).HasComment("知识点Id");
        builder.Property(x => x.KnowledgePointName).HasColumnType("varchar").HasMaxLength(100).HasComment("知识点名称");
        builder.Property(x => x.QuestionCategoryId).IsRequired().HasComment("试题分类Id");
        builder.Property(x => x.QuestionCategoryName).HasColumnType("varchar").HasMaxLength(50).IsRequired().HasComment("试题分类名称");
        builder.Property(x => x.QuestionTypeId).IsRequired().HasComment("试题类型Id");
        builder.Property(x => x.QuestionTypeName).HasMaxLength(20).IsRequired().HasComment("试题类型名称");
        builder.Property(x => x.QuestionText).HasColumnType("longtext").IsRequired().HasComment("试题内容 (题干)");
        builder.Property(x => x.QuestionTitle).HasColumnType("varchar").HasMaxLength(50).IsRequired().HasDefaultValue("").HasComment("试题标题 (管理端使用)");
        builder.Property(x => x.TextAnalysis).HasColumnType("longtext").HasComment("文字解析");
        builder.Property(x => x.MediaAnalysis).HasColumnType("varchar").HasMaxLength(1000).HasComment("音视频解析");
        builder.Property(x => x.FreeState).HasComment("免费分区");
        builder.Property(x => x.EnableState).IsRequired().HasComment("启用状态");
        builder.Property(x => x.Sort).IsRequired().HasComment("排序");
        builder.Property(x => x.QuestionCount).HasDefaultValue(1).HasComment("试题数量");
        builder.Property(x => x.AbilityIds).HasColumnType("varchar").HasMaxLength(360).HasComment("能力维度Id");
        builder.Property(x => x.QuestionOptions).HasColumnType("json").HasComment("试题选项");
        builder.Property(x => x.SubjectTagId).HasComment("试卷标签Id");
        builder.Property(x => x.PaperTagId).HasComment("试卷标签Id");
        builder.Property(x => x.ParentId).HasComment("父题目Id");
        builder.Property(x => x.TrialQuestionId).HasColumnType("varchar(50)").HasComment("命审题Id");

        builder.HasIndex(x => new { x.SubjectId, x.ChapterId, x.SectionId, x.KnowledgePointId, x.QuestionCategoryId, x.TenantId, });

        // 用于案例分析题, 小题查询索引
        builder.HasIndex(x => x.ParentId);

        // 用于试题标题查询
        builder.HasIndex(x => x.QuestionTitle);
    }
}