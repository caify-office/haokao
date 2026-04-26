namespace HaoKao.SalespersonService.Application.Services;

/*
name = "好考助教_欢欢",
userid = "LiuYao_1"

name = "好考助教-芯芯",
userid = "HaoKaoZhuJiao-XinXin"

name = "好考助教-小米",
userid = "HaoKaoZhuJiao-XiaoMi"

name = "好考助教-羊羊",
userid = "HaoKaoZhuJiao_YangYang"

name = "好考助教-茜茜",
userid = "HaoKaoZhuJiao--QianQian"

name = "好考助教-潇潇",
userid = "HaoKaoZhuJiao-XiaoXiao"
*/

/// <summary>
/// 企业微信服务
/// </summary>
public class EnterpriseWeChatService
{
    /// <summary>
    /// 获取AccessToken
    /// https://developer.work.weixin.qq.com/document/path/91039
    /// </summary>
    /// <param name="corpid"></param>
    /// <param name="corpsecret"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    public async Task<AccessTokenModel> GetAccessToken(string corpid, string corpsecret)
    {
        var url = $"https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={corpid}&corpsecret={corpsecret}";
        using var client = HttpClientFactory.Create();
        using var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsAsync<AccessTokenModel>();
        if (result.errcode != 0)
        {
            throw new GirvsException($"获取企业微信AccessToken失败, 错误码: {result.errcode}, 错误信息: {result.errmsg}");
        }
        return result;
    }

    /// <summary>
    /// 获取客户列表
    /// https://developer.work.weixin.qq.com/document/path/92113
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    public async Task<ExternalContactListModel> GetExternalContactList(string userId, string accessToken)
    {
        var url = $"https://qyapi.weixin.qq.com/cgi-bin/externalcontact/list?access_token={accessToken}&userid={userId}";
        using var client = HttpClientFactory.Create();
        using var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsAsync<ExternalContactListModel>();
        if (result.errcode != 0)
        {
            throw new GirvsException($"获取企业微信客户列表失败, 错误码: {result.errcode}, 错误信息: {result.errmsg}");
        }
        return result;
    }

    /// <summary>
    /// 获取客户详情
    /// https://developer.work.weixin.qq.com/document/path/92114
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="externalUserId"></param>
    /// <returns></returns>
    public async Task<ExternalContactModel> GetExternalContact(string accessToken, string externalUserId)
    {
        var url = $"https://qyapi.weixin.qq.com/cgi-bin/externalcontact/get?access_token={accessToken}&external_userid={externalUserId}&cursor=CURSOR";
        using var client = HttpClientFactory.Create();
        using var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsAsync<ExternalContactModel>();
        return result;
    }
}

public record WeiXinBaseModel
{
    /// <summary>
    /// 出错返回码，为0表示成功，非0表示调用失败
    /// </summary>
    public int errcode { get; init; }

    /// <summary>
    /// 返回码提示语
    /// </summary>
    public string errmsg { get; init; }
}

public record AccessTokenModel : WeiXinBaseModel
{
    /// <summary>
    /// 获取到的凭证，最长为512字节
    /// </summary>
    public string access_token { get; init; }

    /// <summary>
    /// 凭证的有效时间（秒）
    /// </summary>
    public int expires_in { get; init; }
}

/// <summary>
/// 客户列表
/// </summary>
public record ExternalContactListModel : WeiXinBaseModel
{
    /// <summary>
    /// 外部联系人的userid列表
    /// </summary>
    public List<string> external_userid { get; init; }
}

/// <summary>
/// 客户详情
/// </summary>
public record ExternalContactModel : WeiXinBaseModel
{
    public ExternalContact external_contact { get; init; }
}

public record ExternalContact
{
    /// <summary>
    /// 外部联系人的userid
    /// </summary>
    public string external_userid { get; init; }

    /// <summary>
    /// 外部联系人的名称
    /// </summary>
    public string name { get; init; }

    /// <summary>
    /// 外部联系人的类型，1表示该外部联系人是微信用户，2表示该外部联系人是企业微信用户
    /// </summary>
    public int type { get; init; }

    /// <summary>
    /// 外部联系人在微信开放平台的唯一身份标识（微信unionid），通过此字段企业可将外部联系人与公众号/小程序用户关联起来。
    /// 仅当联系人类型是微信用户，且企业绑定了微信开发者ID有此字段。
    /// 第三方应用和代开发应用均不可获取，上游企业不可获取下游企业客户的unionid字段
    /// </summary>
    public string unionid { get; init; }
}