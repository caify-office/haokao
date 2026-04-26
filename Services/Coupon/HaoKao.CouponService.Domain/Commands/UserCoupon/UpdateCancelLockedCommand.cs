using System;

namespace HaoKao.CouponService.Domain.Commands.UserCoupon;

/// <summary>
/// 更新是否解锁
/// </summary>
/// <param name="OrderId"></param>
/// <param name="IsLocked"></param>
public record UpdateCancelLockedCommand(Guid OrderId, bool IsLocked) : Command("更新");