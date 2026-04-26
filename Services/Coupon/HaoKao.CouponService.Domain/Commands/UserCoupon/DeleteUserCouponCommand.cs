using System;

namespace HaoKao.CouponService.Domain.Commands.UserCoupon;

/// <summary>
/// 删除命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteUserCouponCommand(
    Guid Id
) : Command("删除");