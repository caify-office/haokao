using FluentValidation;
using System;

namespace HaoKao.CouponService.Domain.Commands.UserCoupon;

/// <summary>
/// 更新是否已使用
/// </summary>
/// <param name="Id"></param>
/// <param name="OrderNumber"></param>
/// <param name="ProductName"></param>
/// <param name="ProductId"></param>
/// <param name="Amount"></param>
/// <param name="FactAmount"></param>
/// <param name="PayTime"></param>
/// <param name="ProductContent"></param>
public record UpdateIsUseCommand(
    Guid Id,
    string OrderNumber,
    string ProductName,
    Guid ProductId,
    decimal Amount,
    decimal FactAmount,
    DateTime PayTime,
    string ProductContent
) : Command("更新")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator) { }
}