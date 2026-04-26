using FluentValidation;
using System;

namespace HaoKao.CouponService.Domain.Commands.MarketingPersonnel;

/// <summary>
/// 更新命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">姓名</param>
/// <param name="TelPhone">手机号码</param>
public record UpdateMarketingPersonnelCommand(
    Guid Id,
    string Name,
    string TelPhone
) : Command("更新")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("姓名不能为空");


        validator.RuleFor(x => TelPhone)
                 .NotEmpty().WithMessage("手机号码不能为空");
    }
}