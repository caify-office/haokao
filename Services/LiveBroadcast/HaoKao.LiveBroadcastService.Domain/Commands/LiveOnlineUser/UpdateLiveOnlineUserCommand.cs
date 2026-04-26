namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveOnlineUser;

/// <summary>
/// 更新直播在线用户命令
/// </summary>
/// <param name="Id">主键Id</param>
/// <param name="IsOnline">是否在线</param>
/// <param name="CreatorName">用户昵称</param>
public record UpdateLiveOnlineUserCommand(Guid Id, bool IsOnline, string CreatorName) : Command("更新直播在线用户命令");