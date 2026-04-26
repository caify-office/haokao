using FluentValidation;
using System;

namespace HaoKao.CouponService.Domain.Commands.Coupon;

/// <summary>
/// 更新领取人数
/// </summary>
/// <param name="Id">主键</param>
public record UpdateRecieveCountCommand(
    Guid Id
) : Command("更新")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator) { }
}