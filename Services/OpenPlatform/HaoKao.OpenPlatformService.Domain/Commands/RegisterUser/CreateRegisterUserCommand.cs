using FluentValidation;

namespace HaoKao.OpenPlatformService.Domain.Commands.RegisterUser;

/// <summary>
/// 创建注册用户命令
/// </summary>
/// <param name="Phone">手机号码</param>
/// <param name="ClientId">客户端Id</param>
/// <param name="ExternalUserCommand"></param>
/// <param name="CreatorId"></param>
/// <param name="NickName"></param>
public record CreateRegisterUserCommand(
    string Phone,
    string ClientId,
    ExternalUserCommand ExternalUserCommand,
    Guid? CreatorId = null,
    string NickName = null
) : Command("创建注册用户")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Phone)
                 .NotEmpty().WithMessage("手机号码不能为空")
                 .Length(11).WithMessage("手机号码长度不正确，请输入11位的手机号码");
    }
}

public record ExternalUserCommand(
    string Scheme,
    string UniqueIdentifier,
    string NickName,
    UserGender UserGender,
    string EmailAddress,
    string HeadImage,
    Dictionary<string, string> OtherInformation
);