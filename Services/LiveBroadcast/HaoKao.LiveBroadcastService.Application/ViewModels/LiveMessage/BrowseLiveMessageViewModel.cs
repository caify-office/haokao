using HaoKao.LiveBroadcastService.Domain.Enums;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;

[AutoMapFrom(typeof(Domain.Entities.LiveMessage))]
[AutoMapTo(typeof(Domain.Entities.LiveMessage))]
public class BrowseLiveMessageViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 是否直播管理员
    /// </summary>
    public bool IsLiveAdmin { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    [JsonProperty("messageType")]
    [JsonPropertyName("messageType")]
    public LiveMessageType LiveMessageType { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [JsonProperty("userId")]
    [JsonPropertyName("userId")]
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    [JsonProperty("userName")]
    [JsonPropertyName("userName")]
    public string CreatorName { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    [JsonProperty("sendTime")]
    [JsonPropertyName("sendTime")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}