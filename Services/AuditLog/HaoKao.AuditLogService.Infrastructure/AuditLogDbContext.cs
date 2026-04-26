using Girvs.EntityFrameworkCore.Context;
using Girvs.EntityFrameworkCore.DbContextExtensions;
using HaoKao.AuditLogService.Domain.Entities;
using HaoKao.AuditLogService.Infrastructure.Mapping;

namespace HaoKao.AuditLogService.Infrastructure;

[GirvsDbConfig("HaoKao_AuditLog")]
public class AuditLogDbContext(DbContextOptions<AuditLogDbContext> options) : GirvsDbContext(options)
{
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuditLogEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}