using System;

namespace HaoKao.CouponService.Domain.Commands.UserCouponPerformance;

/// <summary>
/// 删除命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteUserCouponPerformanceCommand(
    Guid Id
) : Command("删除");