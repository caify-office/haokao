using System.Collections.Generic;

namespace HaoKao.FeedBackService.Domain.Commands.Suggestion;

/// <summary>
/// 回复意见反馈命令
/// </summary>
public record ReplySuggestionCommand() : Command("回复意见反馈命令")
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 回复内容
    /// </summary>
    public string ReplyContent { get; init; }

    /// <summary>
    /// 回复截图
    /// </summary>
    public List<string> ReplyScreenshots { get; init; }

    /// <summary>
    /// Fluent validation
    /// </summary>
    /// <param name="validator"></param>
    /// <typeparam name="TCommand"></typeparam>
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => ReplyContent).NotEmpty().WithMessage("回复内容不能为空");
    }
}