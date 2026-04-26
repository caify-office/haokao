using System.Text.Json.Serialization;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveVideo;

/// <summary>
/// 推流回调
/// </summary>
public class TlCallBackViewModel
{
    /// <summary>
    /// 事件名称
    /// </summary>
    [JsonPropertyName(name: "action")]
    public string Action { get; set; }

    /// <summary>
    /// 推流的客户端IP
    /// </summary>
    [JsonPropertyName(name: "ip")]
    public string Ip { get; set; }

    /// <summary>
    /// 推流流名称
    /// </summary>
    [JsonPropertyName(name: "id")]
    public string Id { get; set; }

    /// <summary>
    /// 推流域名。默认为自定义的推流域名，如果未绑定推流域名即为播流域名
    /// </summary>
    [JsonPropertyName(name: "app")]
    public string App { get; set; }

    /// <summary>
    /// 推流应用名称
    /// </summary>
    [JsonPropertyName(name: "appname")]
    public string Appname { get; set; }

    /// <summary>
    /// Unix时间戳。单位：秒
    /// </summary>
    [JsonPropertyName(name: "time")]
    public string Time { get; set; }

    /// <summary>
    /// 用户推流的参数(生成推流地址时配置)
    /// </summary>
    [JsonPropertyName(name: "usrargs")]
    public string Usrargs { get; set; }

    /// <summary>
    /// CDN接受流的节点或者机器名
    /// </summary>
    [JsonPropertyName(name: "node")]
    public string Node { get; set; }

    /// <summary>
    /// 分辨率的高。单位：像素
    /// </summary>
    [JsonPropertyName(name: "height")]
    public string Height { get; set; }

    /// <summary>
    /// 分辨率的宽。单位：像素
    /// </summary>
    [JsonPropertyName(name: "width")]
    public string Width { get; set; }
}