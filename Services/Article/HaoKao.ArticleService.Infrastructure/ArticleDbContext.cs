using HaoKao.ArticleService.Domain.Entities;

namespace HaoKao.ArticleService.Infrastructure;

[GirvsDbConfig("HaoKao_Article")]
public class ArticleDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleBrowseRecord> ArticleBrowseRecords { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArticleEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleBrowseRecordEntityTypeConfiguration());
    }
}