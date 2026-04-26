using HaoKao.FeedBackService.Domain.Entities;
using HaoKao.FeedBackService.Infrastructure.EntityConfigurations;

namespace HaoKao.FeedBackService.Infrastructure;

[GirvsDbConfig("HaoKao_FeedBack")]
public class FeedBackDbContext(DbContextOptions<FeedBackDbContext> options) : GirvsDbContext(options)
{
    public DbSet<FeedBack> FeedBack { get; set; }

    public DbSet<FeedBackReply> FeedBackReply { get; set; }

    public DbSet<Suggestion> Suggestions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FeedBackEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FeedBackReplyEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SuggestionEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}