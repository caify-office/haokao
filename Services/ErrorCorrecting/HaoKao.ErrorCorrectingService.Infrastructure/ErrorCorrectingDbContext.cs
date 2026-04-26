using HaoKao.ErrorCorrectingService.Domain.Entities;
using HaoKao.ErrorCorrectingService.Infrastructure.EntityConfigurations;

namespace HaoKao.ErrorCorrectingService.Infrastructure;

[GirvsDbConfig("HaoKao_ErrorCorrecting")]
public class ErrorCorrectingDbContext(DbContextOptions<ErrorCorrectingDbContext> options) : GirvsDbContext(options)
{
    public DbSet<ErrorCorrecting> ErrorCorrecting { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ErrorCorrectingEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}