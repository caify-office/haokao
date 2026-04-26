namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveMessage;

/// <summary>
/// 删除聊天消息命令
/// </summary>
/// <param name="Id"></param>
public record DeleteLiveMessageCommand(Guid Id) : Command("删除聊天消息命令");