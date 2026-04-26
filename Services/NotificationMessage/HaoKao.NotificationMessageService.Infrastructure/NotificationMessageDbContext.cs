using HaoKao.NotificationMessageService.Infrastructure.EntityConfigurations;

namespace HaoKao.NotificationMessageService.Infrastructure;

[GirvsDbConfig("HaoKao_NotificationMessage")]
public class NotificationMessageDbContext(DbContextOptions options) : GirvsDbContext(options)
{
    public DbSet<NotificationMessage> NotificationMessages { get; set; }

    public DbSet<WechatMessageSetting> WechatMessageSettings { get; set; }

    public DbSet<MobileMessageSetting> MobileMessageSettings { get; set; }
        
    public DbSet<InSiteMessageSetting> InSiteMessageSettings { get; set; }

    public DbSet<TenantSignSetting> TenantSignSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NotificationMessageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new WechatMessageSettingEntityConfiguration());
        modelBuilder.ApplyConfiguration(new InSiteMessageSettingEntityConfiguration());
        modelBuilder.ApplyConfiguration(new MobileMessageSettingEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TenantSignSettingEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}