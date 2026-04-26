namespace HaoKao.Common.Events.Coupon;

/// <summary>
/// 更新优惠券锁定状态
/// </summary>
/// <param name="OrderId"></param>
public record UpdateIsLockedEvent(Guid OrderId) : IntegrationEvent;