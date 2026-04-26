namespace HaoKao.LiveBroadcastService.Domain.Commands.MutedUser;

/// <summary>
/// 创建禁言用户命令
/// </summary>
/// <param name="Phone">手机号</param>
/// <param name="UserId">用户Id</param>
/// <param name="UserName">用户名称</param>
public record CreateMutedUserCommand(
    string Phone,
    Guid UserId,
    string UserName
) : Command("创建禁言用户")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => UserName)
                 .NotEmpty().WithMessage("用户名称不能为空")
                 .MaximumLength(50).WithMessage("用户名称长度不能大于50")
                 .MinimumLength(2).WithMessage("用户名称长度不能小于2");

        validator.RuleFor(x => Phone)
                 .NotEmpty().WithMessage("手机号不能为空")
                 .MaximumLength(50).WithMessage("手机号长度不能大于50")
                 .MinimumLength(2).WithMessage("手机号长度不能小于2");
    }
}