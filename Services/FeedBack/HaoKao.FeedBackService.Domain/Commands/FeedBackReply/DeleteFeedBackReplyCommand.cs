namespace HaoKao.FeedBackService.Domain.Commands.FeedBackReply;

/// <summary>
/// 删除答疑回复命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteFeedBackReplyCommand(Guid Id) : Command("删除答疑回复");