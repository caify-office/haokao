using HaoKao.BasicService.Domain.Entities;
using HaoKao.BasicService.Infrastructure.EntityConfigurations;

namespace HaoKao.BasicService.Infrastructure;

[GirvsDbConfig("HaoKao_Basic")]
public class BasicDbContext(DbContextOptions<BasicDbContext> options) : GirvsDbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<BasalPermission> BasalPermissions { get; set; }

    public DbSet<ServicePermission> ServicePermissions { get; set; }

    public DbSet<ServiceDataRule> ServiceDataRules { get; set; }

    public DbSet<UserRule> UserRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BasalPermissionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserRulesEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ServicePermissionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceDataRuleEntityTypeConfiguration());
    }
}