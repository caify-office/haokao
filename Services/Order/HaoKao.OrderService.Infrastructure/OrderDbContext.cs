using Girvs.EntityFrameworkCore.Context;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Infrastructure.EntityConfigurations;

namespace HaoKao.OrderService.Infrastructure;

[GirvsDbConfig("HaoKao_Order")]
public class OrderDbContext(DbContextOptions<OrderDbContext> options) : GirvsDbContext(options)
{
    public DbSet<Order> Orders { get; init; }

    public DbSet<PlatformPayer> PlatformPayers { get; init; }

    public DbSet<OrderInvoice> OrderInvoices { get; init; }

    public DbSet<ProductSales> ProductSales { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderInvoiceEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PlatformPayerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductSalesEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}