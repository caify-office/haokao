using Girvs.Configuration;

namespace HaoKao.LiveBroadcastService.Domain.Config;

public class AliLiveConfig : IAppModuleConfig
{
    /// <summary>
    /// 推流域名
    /// </summary>
    public string PushDomain { get; set; } = "tl.haokao123.com";

    /// <summary>
    /// 推流域名配置的鉴权Key
    /// </summary>
    public string PushKey { get; set; } = "iqfSdI0sIH";

    /// <summary>
    /// 播放域名
    /// </summary>
    public string PullDomain { get; set; } = "live.haokao123.com";

    /// <summary>
    /// 播放鉴权Key
    /// </summary>
    public string PullKey { get; set; } = "ysWmO8BBZ0";

    public void Init() { }
}