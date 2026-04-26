using HaoKao.TenantService.Domain.Entities;
using HaoKao.TenantService.Infrastructure.Mappings;

namespace HaoKao.TenantService.Infrastructure;

[GirvsDbConfig("HaoKao_Tenant")]
public class TenantDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
    }
}