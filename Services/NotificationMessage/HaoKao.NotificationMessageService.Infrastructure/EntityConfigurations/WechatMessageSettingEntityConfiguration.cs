using HaoKao.Common.Events.NotificationMessage;

namespace HaoKao.NotificationMessageService.Infrastructure.EntityConfigurations;

public class WechatMessageSettingEntityConfiguration : GirvsAbstractEntityTypeConfiguration<WechatMessageSetting>
{
    public override void Configure(EntityTypeBuilder<WechatMessageSetting> builder)
    {
        var templateConverter = new ValueConverter<List<MessageTemplate>, string>(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<List<MessageTemplate>>(v)
        );

        OnModelCreatingBaseEntityAndTableKey<WechatMessageSetting, Guid>(builder);

        builder.Property(x => x.AppId).HasColumnType("varchar(100)");
        builder.Property(x => x.AppSecret).HasColumnType("varchar(200)");
        builder.Property(x => x.Templates).HasColumnType("json").HasConversion(templateConverter);

        builder.HasData(new WechatMessageSetting
        {
            Id = Guid.Parse("c3032e7b-477f-4922-8054-e61a45610a49"),
            AppId = "wx1917f27b2522e1ce",
            AppSecret = "4fd38646436d0a5110f76ffedfbf90dd",
            Templates =
            [
                new()
                {
                    NotificationMessageType = EventNotificationMessageType.Customize_1,
                    TemplateId = "Gtqw_tKWsvH07P7o-Fvkzgfg78opmDJs-7d696daWOM",
                    Desc = "直播开播提醒\n直播间名称: {{thing6.DATA}}\n开播时间: {{date7.DATA}}"
                },

                new()
                {
                    NotificationMessageType = EventNotificationMessageType.Customize_2,
                    TemplateId = "v3NjkJkYissNTiBO4SDJwrC1A_bQNjuT2zqsXBdXuuM",
                    Desc = "温馨提示: 卡券即将到期，点击查看详情使用\n卡券名称: {{thing2.DATA}}\n过期时间: {{time3.DATA}}"
                },
            ],
        });
    }
}