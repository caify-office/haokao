using HaoKao.BurialPointService.Domain.Entities;
using HaoKao.BurialPointService.Domain.MappingModel;

namespace HaoKao.BurialPointService.Infrastructure;

[GirvsDbConfig("HaoKao_BurialPoint")]
public class BurialPointDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<BurialPoint> BurialPoints { get; set; }

    public DbSet<BrowseRecord> BrowseRecords { get; set; }

    public DbSet<RegisterUser> RegisterUser { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BurialPointEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BrowseRecordEntityTypeConfiguration());
     
    }
}