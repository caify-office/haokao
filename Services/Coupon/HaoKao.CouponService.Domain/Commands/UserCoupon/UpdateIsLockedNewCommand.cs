using FluentValidation;
using System;

namespace HaoKao.CouponService.Domain.Commands.UserCoupon;

/// <summary>
/// 下订单锁定
/// </summary>
/// <param name="CouponIds">优惠券集合ids</param>
/// <param name="OrderId">订单id</param>
public record UpdateIsLockedNewCommand(
    string CouponIds,
    Guid OrderId
) : Command("更新")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator) { }
}