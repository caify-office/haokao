namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 绑定用户手机号
/// </summary>
/// <param name="Id">用户Id</param>
/// <param name="ContactNumber">手机号</param>
public record BindContactNumberCommand(Guid Id, string ContactNumber) : Command("绑定用户手机号")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(user => ContactNumber)
                 .NotEmpty().WithMessage("用户手机号不能为空");
    }
}