using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure;

[GirvsDbConfig("HaoKao_LiveBroadcast")]
public class LiveBroadcastDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<LiveAdministrator> LiveAdministrators { get; set; }

    public DbSet<LiveAnnouncement> LiveAnnouncements { get; set; }

    public DbSet<LivePlayBack> LivePlayBacks { get; set; }

    public DbSet<LiveProductPackage> LiveProductPackages { get; set; }

    public DbSet<LiveCoupon> LiveCoupons { get; set; }


    public DbSet<LiveVideo> LiveVideos { get; set; }

    public DbSet<MutedUser> MutedUsers { get; set; }

    public DbSet<SensitiveWord> SensitiveWords { get; set; }

    public DbSet<LiveMessage> LiveMessages { get; set; }

    public DbSet<LiveComment> LiveComments { get; set; }

    public DbSet<LiveReservation> LiveReservations { get; set; }

    public DbSet<LiveOnlineUser> LiveOnlineUsers { get; set; }

    public DbSet<LiveOnlineUserTrend> LiveOnlineUserTrends { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LiveAdministratorEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveAnnouncementEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LivePlayBackEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveProductPackageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveCouponEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveVideoEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new SensitiveWordEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MutedUserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveCommentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveReservationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveOnlineUserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveOnlineUserTrendEntityTypeConfiguration());
    }
}