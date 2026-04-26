using HaoKao.CourseService.Domain.CoursePracticeModule;

namespace HaoKao.CourseService.Infrastructure.Mappings;

public class CoursePracticeEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<CoursePractice>
{
    public override void Configure(EntityTypeBuilder<CoursePractice> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<CoursePractice, Guid>(builder);

        builder.Property(x => x.CourseChapterName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("课程章节名称");

        builder.Property(x => x.ChapterNodeName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("试题章节名称");

        builder.Property(x => x.QuestionCategoryName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("试题分类名称");

        builder.Property(x => x.QuestionConfig)
               .HasColumnType("json")
               .HasComment("试题配置(智辅学习课程，添加课后练习使用)");

        builder.HasIndex(x => new { x.CourseChapterId, x.KnowledgePointId });
    }
}