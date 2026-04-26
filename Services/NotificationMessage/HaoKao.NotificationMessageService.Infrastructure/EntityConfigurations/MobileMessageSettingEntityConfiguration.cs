using HaoKao.Common.Events.NotificationMessage;

namespace HaoKao.NotificationMessageService.Infrastructure.EntityConfigurations;

public class MobileMessageSettingEntityConfiguration : GirvsAbstractEntityTypeConfiguration<MobileMessageSetting>
{
    public override void Configure(EntityTypeBuilder<MobileMessageSetting> builder)
    {
        var templateConverter = new ValueConverter<List<MessageTemplate>, string>(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<List<MessageTemplate>>(v)
        );

        var listConverter = new ValueConverter<List<string>, string>(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
        );


        OnModelCreatingBaseEntityAndTableKey<MobileMessageSetting, Guid>(builder);

        builder.Property(x => x.AppId).HasColumnType("varchar(100)").HasComment("短信平台 AppId");
        builder.Property(x => x.AppSecret).HasColumnType("varchar(200)").HasComment("短信平台 AppSecret");
        builder.Property(x => x.Templates).HasColumnType("json").HasConversion(templateConverter).HasComment("模板配置");
        builder.Property(x => x.SignList).HasColumnType("varchar(4000)").HasConversion(listConverter).HasComment("签名列表");
        builder.Property(x => x.DefaultSign).HasColumnType("varchar(40)").HasComment("默认签名");
        builder.Property(x => x.SmsSdkAppId).HasColumnType("varchar(100)").HasComment("短信SDk 应用ID");


        builder.HasData(new MobileMessageSetting
        {
            Id = Guid.Parse("d26043f7-c92f-4265-b859-44fcce01212c"),
            MobileMessagePlatform = MobileMessagePlatform.TencentCloud,
            AppId = "AKIDKVVGbCYdS2oMQFbADE9qHSdbKBc2ayR6",
            AppSecret = "rU1JXgeU9dttds1uYzISRAOBj36tGQVW",
            SignList =
            [
                "好慧考",
                "经济师云课堂",
            ],
            DefaultSign = "好慧考",
            Templates =
            [
                new()
                {
                    NotificationMessageType = EventNotificationMessageType.Register,
                    TemplateId = "1073230",
                    Desc = "您正在申请手机注册，验证码为：{1}，{2}分钟内有效！"
                },

                new()
                {
                    NotificationMessageType = EventNotificationMessageType.Login,
                    TemplateId = "1074912",
                    Desc = "{1}为您的登录验证码，请于{2}分钟内填写。"
                },

                new()
                {
                    NotificationMessageType = EventNotificationMessageType.RetrievePassword,
                    TemplateId = "1076233",
                    Desc = "{1}为您的找回密码验证码，请于{2}分钟内填写。"
                },

                new()
                {
                    NotificationMessageType = EventNotificationMessageType.ChangePhoneNumber,
                    TemplateId = "1253909",
                    Desc = "验证码：{1}，您正在进行变更手机号操作，验证码{2}分钟内有效。请勿将验证码告知他人。"
                },

                new()
                {
                    NotificationMessageType = EventNotificationMessageType.CourseUpdate,
                    TemplateId = "1403274",
                    Desc = "您好，您本次报名的{1}，完成{2}环节执行，状态为：{3}，如非本人操作，请忽略本短信。"
                },

                new()
                {
                    NotificationMessageType = EventNotificationMessageType.ProgressReminder,
                    TemplateId = "1220437",
                    Desc = "您好，恭喜您{1} 报名成功，请留意考试时间。"
                },

                new()
                {
                    NotificationMessageType = EventNotificationMessageType.Customize_1,
                    TemplateId = "2121845",
                    Desc = "您预约的“{1}”还有5分钟（{2}）就要开播啦，记得来听课哟！",
                },
                 new()
                {
                    NotificationMessageType = EventNotificationMessageType.Customize_3,
                    TemplateId = "2271962",
                    Desc = "下午好，好慧考小助手查询到您几日未开展学习，马上要临考了，请抓紧时间观看课程哦 ",
                },
                 new()
                {
                    NotificationMessageType = EventNotificationMessageType.Customize_4,
                    TemplateId = "2271964",
                    Desc = "忙完了，请记得每日学习 ",
                },
                 new()
                {
                    NotificationMessageType = EventNotificationMessageType.Customize_5,
                    TemplateId = "2271969",
                    Desc = "每天学习两小时，早日上岸，加油 ",
                },
                 new()
                {
                    NotificationMessageType = EventNotificationMessageType.Customize_6,
                    TemplateId = "2271971",
                    Desc = "学习是每天的头等大事！！",
                },
                  new()
                {
                    NotificationMessageType = EventNotificationMessageType.Customize_7,
                    TemplateId = "2271972",
                    Desc = "好慧考温馨提醒：课程学完了，可进入题库作答巩固知识点哦~ ",
                },
                  new()
                {
                    NotificationMessageType = EventNotificationMessageType.Customize_8,
                    TemplateId = "2332823",
                    Desc = "学员您好，我是好慧考班主任—伊伊。您已购买课程，请添加老师的企业微信（18975172696），以便后续服务。",
                },
                  new()
                {
                    NotificationMessageType = EventNotificationMessageType.Customize_9,
                    TemplateId = "2332824",
                    Desc = "学员您好，我是好慧考班主任—左左。您已购买课程，请添加老师的企业微信（17388982296），以便后续服务。",
                }
            ],
            SmsSdkAppId = "1400559681"
        });
    }
}