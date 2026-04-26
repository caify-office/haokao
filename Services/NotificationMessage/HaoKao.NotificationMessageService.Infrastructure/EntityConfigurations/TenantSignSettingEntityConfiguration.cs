namespace HaoKao.NotificationMessageService.Infrastructure.EntityConfigurations;

public class TenantSignSettingEntityConfiguration : GirvsAbstractEntityTypeConfiguration<TenantSignSetting>
{
    public override void Configure(EntityTypeBuilder<TenantSignSetting> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<TenantSignSetting, Guid>(builder);

        builder.Property(x => x.Sign).HasColumnType("varchar(100)").HasComment("租户签名名称");
    }
}