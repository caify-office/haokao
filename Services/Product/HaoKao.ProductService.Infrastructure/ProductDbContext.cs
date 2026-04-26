using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Infrastructure.Mappings;

namespace HaoKao.ProductService.Infrastructure;

[GirvsDbConfig("HaoKao_Product")]
public class ProductDbContext(DbContextOptions<ProductDbContext> options) : GirvsDbContext(options)
{
    public DbSet<Product> Products { get; init; }

    public DbSet<ProductPackage> ProductPackages { get; init; }

    public DbSet<ProductPermission> ProductPermissions { get; init; }

    public DbSet<AssistantProductPermission> AssistantProductPermissions { get; init; }

    public DbSet<RelatedProduct> RelatedProducts { get; init; }

    public DbSet<StudentPermission> StudentPermissions { get; init; }

    public DbSet<StudentPermissionOperateLog> StudentPermissionOperateLogs { get; init; }

    public DbSet<SupervisorClass> SupervisorClass { get; init; }

    public DbSet<SupervisorStudent> SupervisorStudent { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductPackageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductPermissionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AssistantProductPermissionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RelatedProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StudentPermissionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StudentPermissionOperateLogEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SupervisorClassEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new SupervisorStudentEntityTypeConfiguration());

        modelBuilder.Entity<StudentPermission>()
                    .HasOne(x => x.RegisterUser)
                    .WithOne()
                    .HasForeignKey<StudentPermission>(x => x.StudentId);

        base.OnModelCreating(modelBuilder);
    }
}