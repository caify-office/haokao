using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Mappings;

public class DateAnswerRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<DateAnswerRecord>
{
    public override void Configure(EntityTypeBuilder<DateAnswerRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<DateAnswerRecord, Guid>(builder);

        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.Type).IsRequired().HasComment("类型");
        builder.Property(x => x.Date).IsRequired().HasComment("日期");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("用户Id");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("答题时间");
        builder.Property(x => x.AnswerRecordId).IsRequired().HasComment("答题记录Id");
        builder.Property(x => x.TenantId).IsRequired().HasComment("租户Id");

        builder.HasOne(x => x.AnswerRecord).WithOne()
               .HasForeignKey<DateAnswerRecord>(x => x.AnswerRecordId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.Date, x.SubjectId, x.Type, x.CreatorId, x.TenantId }).IsUnique();
    }
}