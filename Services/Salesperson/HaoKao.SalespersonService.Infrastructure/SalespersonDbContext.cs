using HaoKao.SalespersonService.Domain.Entities;
using HaoKao.SalespersonService.Infrastructure.Mappings;

namespace HaoKao.SalespersonService.Infrastructure;

[GirvsDbConfig("HaoKao_Salesperson")]
public class SalespersonDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<EnterpriseWeChatConfig> EnterpriseWeChatConfigs { get; init; }

    public DbSet<EnterpriseWeChatContact> EnterpriseWeChatContacts { get; init; }

    public DbSet<Salesperson> Salespersons { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EnterpriseWeChatConfigEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EnterpriseWeChatContactEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SalespersonEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}