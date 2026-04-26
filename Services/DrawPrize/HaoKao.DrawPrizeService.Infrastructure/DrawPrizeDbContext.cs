using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Infrastructure;

[GirvsDbConfig("HaoKao_DrawPrize")]
public class DrawPrizeDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<DrawPrize> DrawPrizes { get; set; }
    public DbSet<DrawPrizeRecord> DrawPrizeRecords { get; set; }

    public DbSet<Prize> Prizes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfiguration(new DrawPrizeEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new DrawPrizeRecordEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new PrizeEntityTypeConfiguration());
    }
}