namespace HaoKao.LiveBroadcastService.Domain.Commands.MutedUser;

/// <summary>
/// 删除禁言用户命令
/// </summary>
/// <param name="UserId">用户Id</param>
public record DeleteMutedUserCommand(Guid UserId) : Command("删除禁言用户");