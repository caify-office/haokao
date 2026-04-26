using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Infrastructure.Mappings;

namespace HaoKao.CampaignService.Infrastructure;

[GirvsDbConfig("HaoKao_Campaign")]
public class CampaignDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<GiftBag> GiftBags { get; set; }

    public DbSet<GiftBagReceiveLog> GiftBagReceiveLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GiftBagConfiguration());
        modelBuilder.ApplyConfiguration(new GiftBagReceiveLogConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}