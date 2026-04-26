using Girvs.Infrastructure;
using HaoKao.LiveBroadcastService.Domain.Enums;
using System.Collections.Concurrent;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;

public class OnlineUserState
{
    public ConcurrentDictionary<(Guid UserId, Guid LiveId), HashSet<string>> Connections { get; } = new();

    public readonly ConcurrentDictionary<Guid, int> OnlineCount = new();
}

public record PinTopMessageRequest
{
    public Guid LiveId { get; set; }
}

public record PinTopMessageOutput
{
    /// <summary>
    /// 消息Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 直播间
    /// </summary>
    public Guid LiveId { get; set; }

    /// <summary>
    /// 是否直播管理员
    /// </summary>
    public bool IsLiveAdmin { get; set; }

    /// <summary>
    /// 置顶持续时间
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// 置顶倒计时
    /// </summary>
    public int Countdown { get; set; }

    /// <summary>
    /// 置顶时间
    /// </summary>
    public DateTime PinTopTime { get; set; }

    /// <summary>
    /// 系统时间
    /// </summary>
    public DateTime SystemTime { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string UserName { get; set; }
}

public record LiveMessageInput
{
    public Guid LiveId { get; set; }

    public string Content { get; set; }

    public string UserName { get; set; } = EngineContext.Current.ClaimManager.GetUserName();
}

public record MentionMessageInput : LiveMessageInput
{
    public IReadOnlyList<Guid> MentionUserIds { get; set; }
}

public record ShareLiveInput
{
    public Guid LiveId { get; set; }

    public string UserName { get; set; } = EngineContext.Current.ClaimManager.GetUserName();
}

public record PinTopMessageInput
{
    public Guid LiveId { get; set; }

    public Guid MessageId { get; set; }

    public int Countdown { get; set; }
}

public record LiveMessageOutput
{
    /// <summary>
    /// 消息的Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 直播间Id
    /// </summary>
    public Guid LiveId { get; set; }

    /// <summary>
    /// 消息类型: 0 Chat, 1 Join, 2 Product, 3 Pin
    /// MessagePack 不支持 Enum 类型
    /// </summary>
    public int MessageType { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SendTime { get; set; }
}

public record LiveChatMessageOutput : LiveMessageOutput
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 是否直播管理员
    /// </summary>
    public bool IsLiveAdmin { get; set; }
}

public record LiveStatusChangedInput
{
    public Guid LiveId { get; set; }

    public LiveStatus LiveStatus { get; set; }
}

public record MutedOutput(bool IsMuted);