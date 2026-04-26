using HaoKao.DataDictionaryService.Domain.Entities;
using HaoKao.DataDictionaryService.Infrastructure.Mappings;

namespace HaoKao.DataDictionaryService.Infrastructure;

[GirvsDbConfig("HaoKao_DataDictionary")]
public class DataDictionaryDbContext(DbContextOptions<DataDictionaryDbContext> options) : GirvsDbContext(options)
{
    public DbSet<Dictionaries> dictionaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DictionariesEntityTypeConfiguration());
    }
}