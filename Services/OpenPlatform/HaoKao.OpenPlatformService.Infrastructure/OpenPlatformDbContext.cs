using HaoKao.OpenPlatformService.Infrastructure.Mapping;

namespace HaoKao.OpenPlatformService.Infrastructure;

[GirvsDbConfig("HaoKao_OpenPlatform")]
public class OpenPlatformDbContext(DbContextOptions<OpenPlatformDbContext> options) : GirvsDbContext(options)
{
    public DbSet<RegisterUser> RegisterUsers { get; set; }

    public DbSet<ExternalIdentity> ExternalIdentities { get; set; }

    public DbSet<DailyActiveUserLog> DailyActiveUserLogs { get; set; }

    public DbSet<AccessClient> AccessClients { get; set; }

    /// <summary>
    /// Gets or sets the clients' CORS origins.
    /// </summary>
    /// <value>
    /// The clients CORS origins.
    /// </value>
    public DbSet<AccessClientCorsOrigin> AccessClientCorsOrigins { get; set; }

    public DbSet<AccessClientClaim> AccessClientClaims { get; set; }

    public DbSet<AccessClientGrantType> AccessClientGrantTypes { get; set; }

    public DbSet<AccessClientIdPRestriction> AccessClientIdPRestrictions { get; set; }

    public DbSet<AccessClientPostLogoutRedirectUri> AccessClientPostLogoutRedirectUris { get; set; }

    public DbSet<AccessClientProperty> AccessClientProperties { get; set; }

    public DbSet<AccessClientRedirectUri> AccessClientRedirectUris { get; set; }

    public DbSet<AccessClientScope> AccessClientScopes { get; set; }

    public DbSet<AccessClientSecret> AccessClientSecrets { get; set; }

    public DbSet<AccessClientSigningAlgorithm> AccessClientSigningAlgorithms { get; set; }

    public DbSet<DomainProxy> DomainProxies { get; set; }

    public DbSet<PersistedGrant> PersistedGrants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RegisterUserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ExternalIdentityEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DailyActiveUserLogEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccessClientClaimEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccessClientEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccessClientGrantTypeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccessClientIdPRestrictionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccessClientPostLogoutRedirectUriEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccessClientRedirectUriEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccessClientScopeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccessClientSecretEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccessClientSigningAlgorithmEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DomainProxyEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PersistedGrantEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}