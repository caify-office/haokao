using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Mappings;

public class ProductChapterAnswerRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ProductChapterAnswerRecord>
{
    public override void Configure(EntityTypeBuilder<ProductChapterAnswerRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ProductChapterAnswerRecord, Guid>(builder);

        builder.Property(x => x.ProductId).IsRequired().HasComment("产品Id");
        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.ChapterId).IsRequired().HasComment("章节Id");
        builder.Property(x => x.SectionId).IsRequired().HasComment("小节Id");
        builder.Property(x => x.KnowledgePointId).IsRequired().HasComment("知识点Id");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("用户Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("答题时间");
        builder.Property(x => x.CreateDate).IsRequired().HasComment("答题日期");
        builder.Property(x => x.AnswerRecordId).IsRequired().HasComment("答题记录Id");
        builder.Property(x => x.TenantId).IsRequired().HasComment("租户Id");

        builder.HasOne(x => x.AnswerRecord).WithOne()
               .HasForeignKey<ProductChapterAnswerRecord>(x => x.AnswerRecordId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.ChapterId, x.CreatorId, x.ProductId, x.TenantId });
        builder.HasIndex(x => new { x.ChapterId, x.SubjectId, x.CreatorId, x.ProductId, x.TenantId });
    }
}

public class ProductPaperAnswerRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ProductPaperAnswerRecord>
{
    public override void Configure(EntityTypeBuilder<ProductPaperAnswerRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ProductPaperAnswerRecord, Guid>(builder);

        builder.Property(x => x.ProductId).IsRequired().HasComment("产品Id");
        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.PaperId).IsRequired().HasComment("试卷Id");
        builder.Property(x => x.UserScore).HasPrecision(4, 1).HasComment("用户得分");
        builder.Property(x => x.PassingScore).HasPrecision(4, 1).HasComment("及格分");
        builder.Property(x => x.TotalScore).HasPrecision(4, 1).HasComment("总分");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("用户Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("答题时间");
        builder.Property(x => x.CreateDate).IsRequired().HasComment("答题日期");
        builder.Property(x => x.AnswerRecordId).IsRequired().HasComment("答题记录Id");
        builder.Property(x => x.TenantId).IsRequired().HasComment("租户Id");

        builder.HasOne(x => x.AnswerRecord).WithOne()
               .HasForeignKey<ProductPaperAnswerRecord>(x => x.AnswerRecordId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.PaperId, x.CreatorId, x.ProductId, x.TenantId });
        builder.HasIndex(x => new { x.PaperId, x.SubjectId, x.CreatorId, x.ProductId, x.TenantId });
    }
}

public class ProductKnowledgeAnswerRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ProductKnowledgeAnswerRecord>
{
    public override void Configure(EntityTypeBuilder<ProductKnowledgeAnswerRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ProductKnowledgeAnswerRecord, Guid>(builder);

        builder.Property(x => x.ProductId).IsRequired().HasComment("产品Id");
        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.ChapterId).IsRequired().HasComment("章节Id");
        builder.Property(x => x.SectionId).IsRequired().HasComment("小节Id");
        builder.Property(x => x.KnowledgePointId).IsRequired().HasComment("知识点Id");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("用户Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("答题时间");
        builder.Property(x => x.CreateDate).IsRequired().HasComment("答题日期");
        builder.Property(x => x.CorrectRate).IsRequired().HasComment("正确率");
        builder.Property(x => x.MasteryLevel).IsRequired().HasComment("掌握程度");
        builder.Property(x => x.ExamFrequency).IsRequired().HasComment("考频");
        builder.Property(x => x.AnswerRecordId).IsRequired().HasComment("作答记录Id");
        builder.Property(x => x.TenantId).IsRequired().HasComment("租户Id");

        builder.HasOne(x => x.AnswerRecord).WithOne()
               .HasForeignKey<ProductKnowledgeAnswerRecord>(x => x.AnswerRecordId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.KnowledgePointId, x.CreatorId, x.ProductId, x.TenantId });
        builder.HasIndex(x => new { x.KnowledgePointId, x.SubjectId, x.CreatorId, x.ProductId, x.TenantId });
    }
}