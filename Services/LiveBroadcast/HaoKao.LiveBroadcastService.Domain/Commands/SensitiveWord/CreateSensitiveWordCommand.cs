namespace HaoKao.LiveBroadcastService.Domain.Commands.SensitiveWord;

/// <summary>
/// 创建敏感词命令
/// </summary>
/// <param name="Content">内容</param>
public record CreateSensitiveWordCommand(string Content) : Command("创建敏感词")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Content).NotEmpty().WithMessage("内容不能为空");
    }
}