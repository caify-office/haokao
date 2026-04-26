using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Mappings;

public class PaperAnswerRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<PaperAnswerRecord>
{
    public override void Configure(EntityTypeBuilder<PaperAnswerRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<PaperAnswerRecord, Guid>(builder);

        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.CategoryId).IsRequired().HasComment("类别Id");
        builder.Property(x => x.PaperId).IsRequired().HasComment("试卷Id");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("用户Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("答题时间");
        builder.Property(x => x.UserScore).HasPrecision(4, 1).HasComment("用户得分");
        builder.Property(x => x.PassingScore).HasPrecision(4, 1).HasComment("及格分");
        builder.Property(x => x.TotalScore).HasPrecision(4, 1).HasComment("总分");
        builder.Property(x => x.ElapsedTime).IsRequired().HasComment("答题时间");
        builder.Property(x => x.AnswerRecordId).IsRequired().HasComment("答题记录Id");
        builder.Property(x => x.TenantId).IsRequired().HasComment("租户Id");

        builder.HasOne(x => x.AnswerRecord).WithOne()
               .HasForeignKey<PaperAnswerRecord>(x => x.AnswerRecordId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.PaperId, x.SubjectId, x.CreatorId, x.CategoryId, x.TenantId });
    }
}