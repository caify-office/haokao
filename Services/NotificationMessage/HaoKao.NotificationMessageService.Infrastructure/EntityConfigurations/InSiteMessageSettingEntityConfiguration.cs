using HaoKao.Common.Events.NotificationMessage;

namespace HaoKao.NotificationMessageService.Infrastructure.EntityConfigurations;

public class InSiteMessageSettingEntityConfiguration : GirvsAbstractEntityTypeConfiguration<InSiteMessageSetting>
{
    public override void Configure(EntityTypeBuilder<InSiteMessageSetting> builder)
    {
        var templateConverter = new ValueConverter<List<MessageTemplate>, string>(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<List<MessageTemplate>>(v)
        );

        OnModelCreatingBaseEntityAndTableKey<InSiteMessageSetting, Guid>(builder);

        builder.Property(x => x.AppId).HasColumnType("varchar(100)").HasComment("短信平台 AppId");
        builder.Property(x => x.AppSecret).HasColumnType("varchar(200)").HasComment("短信平台 AppSecret");
        builder.Property(x => x.Templates).HasColumnType("json").HasConversion(templateConverter).HasComment("模板配置");

        builder.HasData(new InSiteMessageSetting
        {
            Id = Guid.Parse("4f579602-853c-482b-893f-48f755e4cc40"),
            AppId = null,
            AppSecret = null,
            Templates =
            [
                new()
                {
                    NotificationMessageType = EventNotificationMessageType.CourseUpdate,
                    TemplateId = "您报考的{0}考试已完成{1}，请继续报考并在报名时间内完成报考。",
                    Desc = "您报考的{0}考试已完成{1}，请继续报考并在报名时间内完成报考。"
                },
            ]
        });
    }
}