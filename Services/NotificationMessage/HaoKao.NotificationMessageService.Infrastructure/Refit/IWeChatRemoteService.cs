namespace HaoKao.NotificationMessageService.Infrastructure.Refit;

[RefitService("WeChatService", false)]
public interface IWeChatRemoteService : IGirvsRefit
{
    [Get("/cgi-bin/token")]
    Task<string> GetAccessTokenAsync(string appid, string secret, string grant_type = "client_credential");

    [Post("/cgi-bin/message/template/send")]
    Task<string> SendMessageAsync(string access_token, [Body] string data);

    [Get("/cgi-bin/template/get_all_private_template")]
    Task<string> GetAllTemplateAsync(string access_key);

    [Get("/sns/oauth2/access_token")]
    Task<string> GetOpenIdAsync(string appid, string secret, string code, string grant_type = "authorization_code");

    [Get("/cgi-bin/user/info")]
    Task<string> GetUserInfo(string access_token, string openid);

    /// <summary>
    /// 微信小程序消息推送
    /// </summary>
    /// <param name="access_token"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    [Post("/cgi-bin/message/subscribe/send")]
    Task<string> SendSubscribeMessage(string access_token, [Body(true)] WeChatSendMessageBody message);
}

public class WeChatSendMessageBody
{
    /// <summary>
    /// 所需下发的订阅模板id
    /// </summary>
    public string Template_id { get; set; }

    /// <summary>
    /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转
    /// </summary>
    public string Page { get; set; } = "index";

    /// <summary>
    /// 接收者（用户）的 openid
    /// </summary>
    public string Touser { get; set; }

    /// <summary>
    /// 模板内容，格式形如 { "key1": { "value": any }, "key2": { "value": any } }的object
    /// </summary>
    public Dictionary<string, Dictionary<string, string>> Data { get; set; }

    /// <summary>
    /// 进入小程序查看”的语言类型，支持zh_CN(简体中文)、en_US(英文)、zh_HK(繁体中文)、zh_TW(繁体中文)，默认为zh_CN
    /// </summary>
    public string Lang { get; set; }

    /// <summary>
    /// 跳转小程序类型：developer为开发版；trial为体验版；formal为正式版；默认为正式版
    /// </summary>
    public string Miniprogram_state { get; set; }
}

public class WeChatResponse
{
    public string Errcode { get; set; }

    public string Errmsg { get; set; }

    public string Access_token { get; set; }

    public string Expires_in { get; set; }

    public DateTime DateTimeNow => DateTime.Now;

    public string Msgid { get; set; }

    public string OpenId { get; set; }
}
