namespace HaoKao.OpenPlatformService.Application.ViewModels.Account;

public record WeiXinUserPhoneNumberBody(string Code);

public record WeiXinAccessTokenBody(string appid, string secret, string grant_type = "client_credential", bool force_refresh = false);

public class WeiXinAccessTokenViewModel
{
    public string Access_token { get; set; }

    public int Expires_in { get; set; }
}

public class WeiXinUserPhoneNumberViewModel
{
    public int Errcode { get; set; }

    public string Errmsg { get; set; }

    public Phoneinfo Phone_info { get; set; }

    public class Phoneinfo
    {
        public string PhoneNumber { get; set; }

        public string PurePhoneNumber { get; set; }

        public int CountryCode { get; set; }

        public Watermark Watermark { get; set; }
    }

    public class Watermark
    {
        public long Timestamp { get; set; }

        public string Appid { get; set; }
    }
}

public record WeiXinGenerateSchemeBody
{
    /// <summary>
    /// 跳转到的目标小程序信息
    /// </summary>
    public WeiXinGenerateSchemeJumpWxa jump_wxa { get; init; }

    /// <summary>
    /// 默认值false，是否生成到期失效的 scheme 码
    /// </summary>
    public bool is_expire { get; init; }

    /// <summary>
    /// 默认值0，到期失效的 scheme 码失效类型，失效时间：0，失效间隔天数：1
    /// </summary>
    public int expire_type { get; init; }

    /// <summary>
    /// 到期失效的 scheme 码的失效时间，为 Unix 时间戳。生成的到期失效 scheme 码在该时间前有效。最长有效期为30天。is_expire 为 true 且 expire_type 为 0 时必填
    /// </summary>
    public long expire_time { get; init; }

    /// <summary>
    /// 到期失效的 scheme 码的失效间隔天数。生成的到期失效 scheme 码在该间隔时间到达前有效。最长间隔天数为30天。is_expire 为 true 且 expire_type 为 1 时必填
    /// </summary>
    public int expire_interval { get; init; }
}

public record WeiXinGenerateSchemeJumpWxa
{
    /// <summary>
    /// 通过 scheme 码进入的小程序页面路径，必须是已经发布的小程序存在的页面，不可携带 query。path 为空时会跳转小程序主页。
    /// </summary>
    public string path { get; init; }

    /// <summary>
    /// 通过 scheme 码进入小程序时的 query，最大1024个字符，只支持数字，大小写英文以及部分特殊字符：!#$'()*+,/:;=?@-._~%`
    /// </summary>
    public string query { get; init; }

    /// <summary>
    /// 默认值"release"。要打开的小程序版本。正式版为"release"，体验版为"trial"，开发版为"develop"，仅在微信外打开时生效。
    /// </summary>
    public string env_version { get; init; } = "release";
}

public record WeiXinGenerateSchemeResponse
{
    /// <summary>
    /// 错误码
    /// </summary>
    public int errcode { get; init; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string errmsg { get; init; }

    /// <summary>
    /// 生成的小程序 scheme 码
    /// </summary>
    public string openlink { get; init; }
}

public record WeiXinCode2SessionResponse
{
    /// <summary>
    /// 会话密钥
    /// </summary>
    public string session_key { get; init; }

    /// <summary>
    /// 用户在开放平台的唯一标识符，若当前小程序已绑定到微信开放平台账号下会返回，详见 UnionID 机制说明。
    /// </summary>
    public string unionid { get; init; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string errmsg { get; init; }

    /// <summary>
    /// 用户唯一标识
    /// </summary>
    public string openid { get; init; }

    /// <summary>
    /// 错误码
    /// </summary>
    public int errcode { get; init; }
}

/// <summary>
/// 查询小程序 scheme 码
/// </summary>
/// <param name="scheme">小程序 scheme 码。支持加密 scheme 和明文 scheme</param>
/// <param name="query_type">查询类型。默认值0，查询 scheme 码信息：0， 查询每天剩余访问次数：1</param>
public record WeiXinQuerySchemeBody(string scheme, int query_type = 0);

public record WeiXinQuerySchemeResponse
{
    /// <summary>
    /// 错误码
    /// </summary>
    public int errcode { get; init; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string errmsg { get; init; }

    /// <summary>
    /// 信息
    /// </summary>
    public WeiXinQuerySchemeInfo scheme_info { get; init; }

    /// <summary>
    /// quota 配置
    /// </summary>
    public WeiXinQuerySchemeQuotaInfo quota_info { get; init; }
}

public record WeiXinQuerySchemeInfo
{
    /// <summary>
    /// 小程序 appid
    /// </summary>
    public string appid { get; init; }

    /// <summary>
    /// 小程序页面路径
    /// </summary>
    public string path { get; init; }

    /// <summary>
    /// 小程序页面query
    /// </summary>
    public string query { get; init; }

    /// <summary>
    /// 创建时间，为 Unix 时间戳
    /// </summary>
    public long create_time { get; init; }

    /// <summary>
    /// 到期失效时间，为 Unix 时间戳，0 表示永久生效
    /// </summary>
    public long expire_time { get; init; }

    /// <summary>
    /// 要打开的小程序版本。正式版为"release"，体验版为"trial"，开发版为"develop"
    /// </summary>
    public string env_version { get; init; }
}

/// <summary>
/// URL Scheme
/// </summary>
/// <param name="remain_visit_quota">URL Scheme（加密+明文）/加密 URL Link 单天剩余访问次数</param>
public record WeiXinQuerySchemeQuotaInfo(string remain_visit_quota);

/// <summary>
/// 生成公众号二维码
/// <see cref="https://developers.weixin.qq.com/doc/offiaccount/Account_Management/Generating_a_Parametric_QR_Code.html"/>
/// </summary>
public record WeiXinOffiAccountQrCodeBody
{
    /// <summary>
    /// 该二维码有效时间，以秒为单位。 最大不超过2592000（即30天），此字段如果不填，则默认有效期为60秒。
    /// </summary>
    public int expire_seconds { get; init; }

    /// <summary>
    /// 二维码类型，QR_SCENE为临时的整型参数值，QR_STR_SCENE为临时的字符串参数值，QR_LIMIT_SCENE为永久的整型参数值，QR_LIMIT_STR_SCENE为永久的字符串参数值
    /// </summary>
    public string action_name { get; init; }

    /// <summary>
    /// 二维码详细信息   
    /// </summary>
    public WeiXinOffiAccountQrCodeActionInfo action_info { get; init; }

    /// <summary>
    /// 二维码详细信息
    /// </summary>
    public record WeiXinOffiAccountQrCodeActionInfo
    {
        public WeiXinOffiAccountQrCodeScene scene { get; init; }

        /// <summary>
        /// 二维码场景
        /// </summary>
        public record WeiXinOffiAccountQrCodeScene
        {
            /// <summary>
            /// 场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）
            /// </summary>
            public int scene_id { get; init; }

            /// <summary>
            /// 场景值ID（字符串形式的ID），字符串类型，长度限制为1到64
            /// </summary>
            public string scene_str { get; init; }
        }
    }
}

/// <summary>
/// 生成公众号二维码返回
/// </summary>
/// <param name="ticket">获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码。</param>
/// <param name="expire_seconds">该二维码有效时间，以秒为单位。 最大不超过2592000（即30天）。</param>
/// <param name="url">二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片</param>
public record WeiXinOffiAccountQrCodeResponse(string ticket, int expire_seconds, string url);

public record WeiXinUserInfoQuery(string access_token, string openid, string lang = "zh_CN");

public record WeiXinUserInfoResponse
{
    /// <summary>
    /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
    /// </summary>
    public int subscribe { get; init; }

    /// <summary>
    /// 用户的标识，对当前公众号唯一
    /// </summary>
    public string openid { get; init; }

    /// <summary>
    /// 用户的语言，简体中文为zh_CN
    /// </summary>
    public string language { get; init; }

    /// <summary>
    /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
    /// </summary>
    public long subscribe_time { get; init; }

    /// <summary>
    /// 用户将公众号绑定到微信开放平台账号后，才会出现该字段。
    /// </summary>
    public string unionid { get; init; }

    /// <summary>
    /// 公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
    /// </summary>
    public string remark { get; init; }

    /// <summary>
    /// 用户所在的分组ID（兼容旧的用户分组接口）
    /// </summary>
    public int groupid { get; init; }

    /// <summary>
    /// 用户被打上的标签ID列表
    /// </summary>
    public int[] tagid_list { get; init; }

    /// <summary>
    /// 返回用户关注的渠道来源，
    /// ADD_SCENE_SEARCH 公众号搜索，
    /// ADD_SCENE_ACCOUNT_MIGRATION 公众号迁移，
    /// ADD_SCENE_PROFILE_CARD 名片分享，
    /// ADD_SCENE_QR_CODE 扫描二维码，
    /// ADD_SCENE_PROFILE_LINK 图文页内名称点击，
    /// ADD_SCENE_PROFILE_ITEM 图文页右上角菜单，
    /// ADD_SCENE_PAID 支付后关注，
    /// ADD_SCENE_WECHAT_ADVERTISEMENT 微信广告，
    /// ADD_SCENE_REPRINT 他人转载，
    /// ADD_SCENE_LIVESTREAM 视频号直播，
    /// ADD_SCENE_CHANNELS 视频号，
    /// ADD_SCENE_WXA 小程序关注，
    /// ADD_SCENE_OTHERS 其他
    /// </summary>
    public string subscribe_scene { get; init; }

    /// <summary>
    /// 二维码扫码场景（开发者自定义）
    /// </summary>
    public int qr_scene { get; init; }

    /// <summary>
    /// 二维码扫码场景描述（开发者自定义）
    /// </summary>
    public string qr_scene_str { get; init; }
}

/// <summary>
/// 微信公众号 通过扫二维码 关注/取消事件
/// </summary>
public record WeiXinOffiAccountQrCodeSubscribeEvent
{
    /// <summary>
    /// 开发者微信号
    /// </summary>
    public string ToUserName { get; init; }

    /// <summary>
    /// 发送方账号（一个OpenID）
    /// </summary>
    public string FromUserName { get; init; }

    /// <summary>
    /// 消息创建时间 （整型）
    /// </summary>
    public long CreateTime { get; init; }

    /// <summary>
    /// 消息类型，event
    /// </summary>
    public string MsgType { get; init; }

    /// <summary>
    /// 事件类型，subscribe(订阅)、unsubscribe(取消订阅)
    /// </summary>
    public string Event { get; init; }

    /// <summary>
    /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
    /// </summary>
    public string EventKey { get; init; }

    /// <summary>
    /// 二维码的ticket，可用来换取二维码图片
    /// </summary>
    public string Ticket { get; init; }
}
