using HaoKao.ContinuationService.Domain.ContinuationAuditModule;
using HaoKao.ContinuationService.Domain.ContinuationSetupModule;
using HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;
using HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;
using HaoKao.ContinuationService.Infrastructure.Mappings;

namespace HaoKao.ContinuationService.Infrastructure;

[GirvsDbConfig("HaoKao_Continuation")]
public class ContinuationAuditRecordDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<ContinuationAudit> ContinuationAudits { get; set; }

    public DbSet<ContinuationSetup> ContinuationSetups { get; set; }

    public DbSet<ProductExtensionPolicy> ProductExtensionPolicies { get; set; }

    public DbSet<ProductExtensionRequest> ProductExtensionRequests { get; set; }

    public DbSet<ProductExtensionAuditLog> ProductExtensionAuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ContinuationAuditEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ContinuationSetupEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new ProductExtensionPolicyEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductExtensionRequestEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductExtensionAuditLogEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}