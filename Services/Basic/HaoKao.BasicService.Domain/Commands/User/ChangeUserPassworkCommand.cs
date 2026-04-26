namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 修改用户密码
/// </summary>
/// <param name="Id">用户Id</param>
/// <param name="NewPassword">新密码</param>
public record ChangeUserPasswordCommand(Guid Id, string NewPassword) : Command("修改用户密码")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(user => NewPassword)
                 .NotEmpty().WithMessage("用户新的登陆密码不能为空")
                 .MinimumLength(6).WithMessage("用户新的登陆密码长度不能小于6");
    }
}