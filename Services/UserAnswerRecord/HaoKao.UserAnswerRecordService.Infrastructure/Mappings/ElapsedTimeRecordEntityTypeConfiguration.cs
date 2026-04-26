using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Mappings;

public class ElapsedTimeRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ElapsedTimeRecord>
{
    public override void Configure(EntityTypeBuilder<ElapsedTimeRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ElapsedTimeRecord, Guid>(builder);

        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.TargetId).IsRequired().HasComment("目标Id");
        builder.Property(x => x.ProductId).IsRequired().HasComment("产品Id");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("用户Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("做题时间");
        builder.Property(x => x.CreateDate).IsRequired().HasComment("做题日期");
        builder.Property(x => x.Type).IsRequired().HasComment("类型");
        builder.Property(x => x.QuestionCount).IsRequired().HasComment("总题数");
        builder.Property(x => x.AnswerCount).IsRequired().HasComment("答题数");
        builder.Property(x => x.CorrectCount).IsRequired().HasComment("正确数");
        builder.Property(x => x.ElapsedSeconds).IsRequired().HasComment("耗时(秒)");
        builder.Property(x => x.TenantId).IsRequired().HasComment("租户Id");

        builder.HasIndex(x => new { x.CreatorId, x.SubjectId, x.Type, x.ProductId, x.TenantId });
    }
}