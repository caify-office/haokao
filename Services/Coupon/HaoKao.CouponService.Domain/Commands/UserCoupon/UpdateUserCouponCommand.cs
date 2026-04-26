using FluentValidation;
using System;

namespace HaoKao.CouponService.Domain.Commands.UserCoupon;

/// <summary>
/// 更新命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="IsUse">是否使用 0-未使用 1-已使用</param>
/// <param name="OrderNo">订单编号</param>
/// <param name="OrderId">订单id</param>
/// <param name="ProductName">产品名称</param>
/// <param name="Amount">产品名称</param>
/// <param name="PayTime">产品名称</param>
public record UpdateUserCouponCommand(
    Guid Id,
    bool IsUse,
    string OrderNo,
    Guid OrderId,
    string ProductName,
    decimal Amount,
    DateTime PayTime
) : Command("更新")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator) { }
}