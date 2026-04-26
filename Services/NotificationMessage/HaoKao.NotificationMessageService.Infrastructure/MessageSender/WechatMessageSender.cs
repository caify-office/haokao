using HaoKao.NotificationMessageService.Domain.MessageSenders;
using HaoKao.NotificationMessageService.Infrastructure.Refit;

namespace HaoKao.NotificationMessageService.Infrastructure.MessageSender;

public class WechatMessageSender : IWechatMessageSender
{
    public WechatMessageSetting Setting { get; set; }

    public RegisteredUser RegisteredUser { get; set; }

    public Dictionary<string, string> Parameter { get; set; }

    private Task<WeChatResponse> GetAccessTokenResponse()
    {
        var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
        var cacheKey = GirvsEntityCacheDefaults<WechatMessageSetting>.ByIdCacheKey.Create($"{Setting.AppId}:access_token", cacheTime: 119);
        return cacheManager.GetAsync(cacheKey, async () =>
        {
            var remoteService = EngineContext.Current.RestService<IWeChatRemoteService>();
            var tokenResponseString = await remoteService.GetAccessTokenAsync(Setting.AppId, Setting.AppSecret);
            return JsonConvert.DeserializeObject<WeChatResponse>(tokenResponseString);
        });
    }

    public async Task<(NotificationMessageSendState, string)> SendAsync(MessageTemplate messageTemplate)
    {
        var body = new WeChatSendMessageBody
        {
            Touser = RegisteredUser.OpenId,
            Template_id = messageTemplate.TemplateId,
            Data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(Parameter[nameof(WeChatSendMessageBody.Data)]),
            Miniprogram_state = Parameter[nameof(WeChatSendMessageBody.Miniprogram_state)],
            Lang = Parameter[nameof(WeChatSendMessageBody.Lang)],
            Page = Parameter[nameof(WeChatSendMessageBody.Page)],
        };

        var (result, message) = await SendWeChatMessageAsync();

        Parameter = body.Data.Select(x => new KeyValuePair<string, string>(x.Key, x.Value["value"])).ToDictionary(x => x.Key, x => x.Value);

        return (result ? NotificationMessageSendState.SendSuccess : NotificationMessageSendState.SendFail, message);

        async Task<(bool, string)> SendWeChatMessageAsync()
        {
            try
            {
                var remoteService = EngineContext.Current.RestService<IWeChatRemoteService>();
                var tokenResponse = await GetAccessTokenResponse();

                if (string.IsNullOrWhiteSpace(tokenResponse.Access_token)) return (false, $"发送失败：没有有效的发送消息的token,{tokenResponse.Errmsg}");

                var sendResultJson = await remoteService.SendSubscribeMessage(tokenResponse.Access_token, body);
                var response = JsonConvert.DeserializeObject<WeChatResponse>(sendResultJson);
                if (response?.Errcode == "0" && response.Errmsg.ToLower() == "ok")
                {
                    return (true, $"发送成功：msgid-{response.Msgid}");
                }

                return (false, $"发送失败：{response?.Errcode}: {response?.Errmsg}");
            }
            catch (Exception ex)
            {
                return (false, $"发送失败：{ex.Message}");
            }
        }
    }
}