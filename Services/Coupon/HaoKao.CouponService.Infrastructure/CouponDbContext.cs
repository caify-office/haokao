using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Infrastructure.EntityConfigurations;

namespace HaoKao.CouponService.Infrastructure;

[GirvsDbConfig("HaoKao_Coupon")]
public class CouponDbContext(DbContextOptions<CouponDbContext> options) : GirvsDbContext(options)
{
    public DbSet<UserCouponPerformance> UserCouponPerformances { get; set; }

    public DbSet<Coupon> Coupons { get; set; }

    public DbSet<UserCoupon> UserCoupons { get; set; }

    public DbSet<MarketingPersonnel> MarketingPersonnels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserCouponPerformanceEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CouponEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserCouponEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MarketingPersonnelEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}