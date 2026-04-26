namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveOnlineUser;

/// <summary>
/// 创建直播在线用户命令
/// </summary>
/// <param name="LiveId">直播Id</param>
/// <param name="Phone">手机号</param>
public record CreateLiveOnlineUserCommand(Guid LiveId, string Phone) : Command("创建直播在线用户命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Phone)
                 .NotEmpty().WithMessage("手机号不能为空")
                 .MaximumLength(50).WithMessage("手机号长度不能大于50")
                 .MinimumLength(2).WithMessage("手机号长度不能小于2");
    }
}