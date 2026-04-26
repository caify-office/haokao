using HaoKao.NotificationMessageService.Domain.MessageSenders;

namespace HaoKao.NotificationMessageService.Infrastructure.MessageSender;

public class MobileMessageSender : IMobileMessageSender
{
    public MobileMessageSetting Setting { get; set; }

    public RegisteredUser RegisteredUser { get; set; }

    public string[] Parameter { get; set; }

    public TenantSignSetting TenantSignSetting { get; set; }

    public string PhoneNumber { get; set; }

    private async Task<string> GetTemplateContentAsync(string templateId)
    {
        var templates = await GetTemplatesAsync(templateId);
        return templates.FirstOrDefault()?.TemplateContent;
    }

    private SmsClient BuilderSmsClient()
    {
        return new SmsClient(
            new Credential
            {
                SecretId = Setting.AppId,
                SecretKey = Setting.AppSecret
            },
            "ap-guangzhou",
            new ClientProfile
            {
                HttpProfile = new HttpProfile
                {
                    Endpoint = "sms.tencentcloudapi.com"
                }
            }
        );
    }

    private async Task<List<DescribeTemplateListStatus>> GetTemplatesAsync(string templateId)
    {
        var templateIds = new List<ulong?>
        {
            Convert.ToUInt64(templateId)
        };
        var client = BuilderSmsClient();
        var req = new DescribeSmsTemplateListRequest
        {
            TemplateIdSet = templateIds.ToArray(),
            International = 0
        };
        var resp = await client.DescribeSmsTemplateList(req);
        return resp.DescribeTemplateStatusSet.ToList();
    }

    private Task<(bool, string)> SendTextAsync(string templateId, string[] phoneNumbers, string[] templateParams, string signName)
    {
        var client = BuilderSmsClient();

        var req = new SendSmsRequest
        {
            SmsSdkAppId = Setting.SmsSdkAppId,
            TemplateId = templateId,
            PhoneNumberSet = phoneNumbers,
            TemplateParamSet = templateParams,
            SignName = signName
        };

        var resp = client.SendSmsSync(req);

        var obj = resp.SendStatusSet.FirstOrDefault();
        var isOK = obj?.Code.ToUpper();
        return Task.FromResult(!"OK".Equals(isOK) ? (false, $"发送失败,原因{obj.Message}") : (true, "发送成功"));
    }

    public async Task<(NotificationMessageSendState, string)> SendAsync(MessageTemplate messageTemplate)
    {
        var signName = string.IsNullOrWhiteSpace(TenantSignSetting.Sign)
            ? Setting.DefaultSign
            : TenantSignSetting.Sign;

        var phoneNumbers = new[]
        {
            PhoneNumber
        };

        var (result, message) = await SendTextAsync(messageTemplate.TemplateId, phoneNumbers, Parameter, signName);
        return (result ? NotificationMessageSendState.SendSuccess : NotificationMessageSendState.SendFail, message);
    }
}