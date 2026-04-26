using HaoKao.QuestionService.Domain.QuestionWrongPaperMoudle;

namespace HaoKao.QuestionService.Infrastructure.Mappings;

public class QuestionWrongPaperEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<QuestionWrongPaper>
{
    public override void Configure(EntityTypeBuilder<QuestionWrongPaper> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<QuestionWrongPaper, Guid>(builder);

        builder.Property(x => x.SubjectId).IsRequired().HasComment("科目Id");
        builder.Property(x => x.PaperName).IsRequired().HasMaxLength(100).HasComment("试卷名称");
        builder.Property(x => x.DownloadUrl).IsRequired().HasColumnType("text").HasComment("下载地址");
        builder.Property(x => x.QuestionCount).IsRequired().HasDefaultValue(0).HasComment("试题数量");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");
        builder.Property(x => x.CreatorId).IsRequired().HasComment("创建人Id");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.SubjectId, x.CreatorId, x.CreateTime, x.TenantId }).IsUnique();
    }
}