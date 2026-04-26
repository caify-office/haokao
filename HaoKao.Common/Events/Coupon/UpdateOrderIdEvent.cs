namespace HaoKao.Common.Events.Coupon;

/// <summary>
/// 添加订单更改用户优惠券orderId
/// </summary>
/// <param name="CouponIds">优惠券id</param>
/// <param name="OrderId">订单id</param>
public record UpdateOrderIdEvent(string CouponIds, Guid OrderId) : IntegrationEvent;