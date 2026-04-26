using FluentValidation;
using System;

namespace HaoKao.CouponService.Domain.Commands.UserCouponPerformance;

/// <summary>
/// 更新命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="PersonName">助教实名名称</param>
/// <param name="PersonUserId">营销助教userid</param>
public record UpdateUserCouponPerformanceCommand(
    Guid Id,
    string PersonName,
    Guid PersonUserId
) : Command("更新")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator) { }
}