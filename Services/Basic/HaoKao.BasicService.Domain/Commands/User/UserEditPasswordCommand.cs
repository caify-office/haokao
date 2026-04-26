using Girvs;

namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 用户自身修改密码
/// </summary>
/// <param name="Id">用户Id</param>
/// <param name="OldPassword">旧密码</param>
/// <param name="NewPassword">新密码</param>
public record UserEditPasswordCommand(Guid Id, string OldPassword, string NewPassword) : Command("用户自身修改密码")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(user => OldPassword)
                 .NotEmpty().WithMessage("用户旧的登陆密码不能为空")
                 .MinimumLength(6).WithMessage("用户旧的登陆密码长度不能小于6");
        validator.RuleFor(user => NewPassword)
                 .NotEmpty().WithMessage("用户新的登陆密码不能为空")
                 .MinimumLength(6).WithMessage("用户新的登陆密码长度不能小于6")
                 .Must(x => CommonHelper.Complexity(NewPassword))
                 .WithMessage("登陆密码必须包含数字、小写或大写字母、特殊字符");
    }
}