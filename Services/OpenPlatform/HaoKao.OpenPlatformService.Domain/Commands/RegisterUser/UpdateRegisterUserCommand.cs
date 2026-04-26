using FluentValidation;

namespace HaoKao.OpenPlatformService.Domain.Commands.RegisterUser;

/// <summary>
/// 更新注册用户
/// </summary>
/// <param name="Id"></param>
/// <param name="Phone">手机号码</param>
/// <param name="Password">密码</param>
/// <param name="UserGender">用户性别</param>
/// <param name="NickName">用户昵称</param>
/// <param name="UserState"></param>
/// <param name="EmailAddress">邮箱地址</param>
/// <param name="HeadImage">用户头像</param>
public record UpdateRegisterUserCommand(
    Guid Id,
    string Phone,
    string Password,
    UserGender UserGender,
    string NickName,
    UserState UserState,
    string EmailAddress,
    string HeadImage
) : Command("更新注册用户")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Phone)
                 .NotEmpty().WithMessage("手机号码不能为空")
                 .MaximumLength(50).WithMessage("手机号码长度不能大于50")
                 .MinimumLength(2).WithMessage("手机号码长度不能小于2");

        validator.RuleFor(x => NickName)
                 .MaximumLength(30).WithMessage("用户昵称长度不能超过30");

        validator.RuleFor(x => EmailAddress)
                 .MaximumLength(40).WithMessage("用户昵称长度不能超过40");

        validator.RuleFor(x => HeadImage)
                 .MaximumLength(200).WithMessage("用户昵称长度不能超过200");
    }
}