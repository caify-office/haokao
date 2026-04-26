namespace HaoKao.FeedBackService.Domain.Commands.Suggestion;

/// <summary>
/// 将意见反馈变更为完结命令
/// </summary>
/// <param name="Id"></param>
public record CloseSuggestionCommand(Guid Id) : Command("将意见反馈变更为完结命令");