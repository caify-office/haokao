using HaoKao.ArticleService.Domain.Entities;

namespace HaoKao.ArticleService.Infrastructure.EntityConfigurations;

public class ArticleEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Article>
{
    public override void Configure(EntityTypeBuilder<Article> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Article, Guid>(builder);

        builder.Property(x => x.Title)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("标题");

        builder.Property(x => x.Content)
            .HasColumnType("mediumtext")
            .HasComment("内容");

        builder.Property(x => x.IsExternalURL)
       .HasDefaultValue(false)
       .HasComment("是否外部链接");

        builder.Property(x => x.PreviewUrl)
            .HasColumnType("varchar")
            .HasMaxLength(500)
            .HasComment("预览图");

        builder.HasIndex(x => 
        new 
        {
            x.Column,
            x.Category, 
            x.TenantId 
        });

    }
}