using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Mappings;

public class UserAnswerRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<UserAnswerRecord>
{
    public override void Configure(EntityTypeBuilder<UserAnswerRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<UserAnswerRecord, Guid>(builder);

        builder.Property(x => x.SubjectId)
               .IsRequired()
               .HasComment("科目Id");

        builder.Property(x => x.QuestionCategoryId)
               .IsRequired()
               .HasComment("分类Id");

        builder.Property(x => x.CreatorId)
               .IsRequired()
               .HasComment("用户Id");

        builder.Property(x => x.RecordIdentifier)
               .IsRequired()
               .HasComment("答题标识符 章节Id，或试卷Id，每日一练为Guid.Empty");

        builder.Property(x => x.RecordIdentifierName)
               .HasColumnType("varchar(100)")
               .HasComment("答题标识符名称");

        builder.Property(x => x.UserScore)
               .HasPrecision(4, 1)
               .HasComment("用户得分");

        builder.Property(x => x.PassingScore)
               .HasPrecision(4, 1)
               .HasComment("及格分数");

        builder.Property(x => x.TotalScore)
               .HasPrecision(4, 1)
               .HasComment("试题总分");

        builder.Property(x => x.AnswerType).HasComment("答题类型");
        builder.Property(x => x.ElapsedTime).HasComment("耗时");
        builder.Property(x => x.QuestionCount).HasComment("总题数");
        builder.Property(x => x.CorrectCount).HasComment("正确数");
        builder.Property(x => x.AnswerCount).HasComment("作答数");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new
        {
            x.SubjectId,
            x.QuestionCategoryId,
            x.RecordIdentifier,
            x.CreatorId,
            x.CreateTime,
            x.TenantId
        });
    }
}