namespace HaoKao.LiveBroadcastService.Domain.Commands.SensitiveWord;

/// <summary>
/// 更新敏感词命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Content">内容</param>
public record UpdateSensitiveWordCommand(Guid Id, string Content) : Command("更新敏感词")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Content).NotEmpty().WithMessage("内容不能为空");
    }
}