namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveAdministrator;

/// <summary>
/// 创建直播管理员命令
/// </summary>
/// <param name="UserId">用户Id</param>
/// <param name="Name">姓名</param>
/// <param name="Phone">手机号</param>
public record CreateLiveAdministratorCommand(
    Guid UserId,
    string Name,
    string Phone
) : Command("创建直播管理员")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("姓名不能为空")
                 .MaximumLength(50).WithMessage("姓名长度不能大于50");

        validator.RuleFor(x => Phone)
                 .NotEmpty().WithMessage("手机号不能为空")
                 .MaximumLength(50).WithMessage("手机号长度不能大于50")
                 .MinimumLength(2).WithMessage("手机号长度不能小于2");
    }
}