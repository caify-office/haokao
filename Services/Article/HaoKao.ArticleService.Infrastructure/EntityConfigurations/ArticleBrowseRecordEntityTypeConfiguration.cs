using HaoKao.ArticleService.Domain.Entities;

namespace HaoKao.ArticleService.Infrastructure.EntityConfigurations;

public class ArticleBrowseRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ArticleBrowseRecord>
{
    public override void Configure(EntityTypeBuilder<ArticleBrowseRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ArticleBrowseRecord, Guid>(builder);

        builder.HasIndex(x => new { x.ArticleId, x.ClientUniqueId, x.TenantId });
    }
}