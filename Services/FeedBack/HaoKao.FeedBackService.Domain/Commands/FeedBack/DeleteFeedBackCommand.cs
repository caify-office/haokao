namespace HaoKao.FeedBackService.Domain.Commands.FeedBack;

/// <summary>
/// 删除命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteFeedBackCommand(Guid Id) : Command("删除");