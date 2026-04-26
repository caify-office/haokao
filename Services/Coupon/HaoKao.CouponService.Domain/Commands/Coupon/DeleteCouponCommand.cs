using System;

namespace HaoKao.CouponService.Domain.Commands.Coupon;

/// <summary>
/// 删除命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteCouponCommand(
    Guid Id
) : Command("删除");