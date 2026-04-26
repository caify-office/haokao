using FluentValidation;

namespace HaoKao.CouponService.Domain.Commands.MarketingPersonnel;

/// <summary>
/// 创建命令
/// </summary>
/// <param name="Name">姓名</param>
/// <param name="TelPhone">手机号码</param>
public record CreateMarketingPersonnelCommand(
    string Name,
    string TelPhone
) : Command("创建")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("姓名不能为空");

        validator.RuleFor(x => TelPhone)
                 .NotEmpty().WithMessage("手机号码不能为空");
    }
}