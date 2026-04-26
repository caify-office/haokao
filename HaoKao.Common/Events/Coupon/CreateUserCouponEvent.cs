namespace HaoKao.Common.Events.Coupon;

public record CreateUserCouponEvent(
    Guid CouponId,
    string NickName,
    int ChannelType
) : IntegrationEvent;