using HaoKao.FeedBackService.Domain.Enums;

namespace HaoKao.FeedBackService.Domain.Commands.FeedBack;

/// <summary>
/// 更新命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Status">状态</param>
public record UpdateFeedBackCommand(Guid Id, StatusEnum Status) : Command("更新");