using System.Collections.Generic;

namespace HaoKao.FeedBackService.Domain.Commands.Suggestion;

public record CreateSuggestionCommand() : Command("创建意见反馈命令")
{
    /// <summary>
    /// 反馈类型
    /// </summary>
    public string SuggestionType { get; init; }

    /// <summary>
    /// 反馈来源
    /// </summary>
    public string SuggestionFrom { get; init; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; init; }

    /// <summary>
    /// 问题描述
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// 相关截图
    /// </summary>
    public List<string> Screenshots { get; init; }

    /// <summary>
    /// Fluent validation
    /// </summary>
    /// <param name="validator"></param>
    /// <typeparam name="TCommand"></typeparam>
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => SuggestionType).NotEmpty().WithMessage("反馈类型不能为空");
        validator.RuleFor(x => SuggestionFrom).NotEmpty().WithMessage("反馈来源不能为空");
        validator.RuleFor(x => Phone).NotEmpty().WithMessage("手机号不能为空");
        validator.RuleFor(x => Description).NotEmpty().WithMessage("问题描述不能为空");
    }
}