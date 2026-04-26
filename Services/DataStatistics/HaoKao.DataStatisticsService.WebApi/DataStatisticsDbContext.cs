using HaoKao.DataStatisticsService.WebApi.Models;

namespace HaoKao.DataStatisticsService.WebApi;

[GirvsDbConfig("HaoKao_DataStatistics")]
public class DataStatisticsDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<ProgressStatistics> ProgressStatistics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProgressStatistics>().ToView("ProgressStatistics").HasNoKey();
    }
}