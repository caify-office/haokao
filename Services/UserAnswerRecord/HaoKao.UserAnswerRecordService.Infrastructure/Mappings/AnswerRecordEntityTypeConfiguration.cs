using HaoKao.Common.Extensions;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Mappings;

public class AnswerRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<AnswerRecord>
{
    public override void Configure(EntityTypeBuilder<AnswerRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AnswerRecord, Guid>(builder);

        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.QuestionCount).IsRequired().HasComment("总题数");
        builder.Property(x => x.AnswerCount).IsRequired().HasComment("答题数");
        builder.Property(x => x.CorrectCount).IsRequired().HasComment("正确数");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("用户Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("答题时间");
        builder.Property(x => x.TenantId).IsRequired().HasComment("租户Id");

        builder.HasIndex(x => new { x.SubjectId, x.CreatorId, x.TenantId });
    }
}

public class AnswerQuestionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<AnswerQuestion>
{
    public override void Configure(EntityTypeBuilder<AnswerQuestion> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AnswerQuestion, Guid>(builder);

        builder.Property(x => x.RecordId).IsRequired().HasComment("作答记录Id");
        builder.Property(x => x.QuestionId).IsRequired().HasComment("试题Id");
        builder.Property(x => x.QuestionTypeId).IsRequired().HasComment("试题类型Id");
        builder.Property(x => x.UserAnswers).HasColumnType("json").IsRequired().HasComment("用户作答")
               .HasConversion(JsonConversion.Create<List<UserAnswer>>());
        builder.Property(x => x.JudgeResult).IsRequired().HasComment("判题结果");
        builder.Property(x => x.WhetherMark).IsRequired().HasComment("是否标记");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("用户Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("答题时间");
        builder.Property(x => x.ParentId).HasComment("试题父Id");
        builder.Property(x => x.TenantId).IsRequired().HasComment("租户Id");

        builder.HasOne<AnswerRecord>().WithMany(x => x.Questions).HasForeignKey(x => x.RecordId);

        builder.HasIndex(x => new { x.QuestionId, x.CreatorId, x.CreateTime, x.TenantId });
    }
}