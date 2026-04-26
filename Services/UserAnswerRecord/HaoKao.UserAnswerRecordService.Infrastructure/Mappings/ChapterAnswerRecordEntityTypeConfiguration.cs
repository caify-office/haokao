using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Mappings;

public class ChapterAnswerRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ChapterAnswerRecord>
{
    public override void Configure(EntityTypeBuilder<ChapterAnswerRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ChapterAnswerRecord, Guid>(builder);

        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.CategoryId).IsRequired().HasComment("分类Id");
        builder.Property(x => x.ChapterId).IsRequired().HasComment("章节Id");
        builder.Property(x => x.SectionId).IsRequired().HasComment("小节Id");
        builder.Property(x => x.KnowledgePointId).IsRequired().HasComment("知识点Id");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("用户Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("答题时间");
        builder.Property(x => x.AnswerRecordId).IsRequired().HasComment("作答记录Id");
        builder.Property(x => x.TenantId).IsRequired().HasComment("租户Id");

        builder.HasOne(x => x.AnswerRecord).WithOne()
               .HasForeignKey<ChapterAnswerRecord>(x => x.AnswerRecordId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.SubjectId, x.CreatorId, x.CategoryId, x.TenantId });
        builder.HasIndex(x => new { x.SectionId, x.CreatorId, x.CategoryId, x.TenantId });
        builder.HasIndex(x => new { x.KnowledgePointId, x.CreatorId, x.CategoryId, x.TenantId });
        builder.HasIndex(x => new { x.KnowledgePointId, x.SectionId, x.ChapterId, x.SubjectId, x.CreatorId, x.CategoryId, x.TenantId });
    }
}