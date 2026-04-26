namespace HaoKao.OrderService.Domain.Enums;

/// <summary>
/// 订单状态
/// </summary>
public enum OrderState
{
    /// <summary>
    /// 支付中
    /// </summary>
    Paying,

    /// <summary>
    /// 支付失败
    /// </summary>
    PaymentFailed,

    /// <summary>
    /// 支付成功
    /// </summary>
    PaymentSuccessful,

    /// <summary>
    /// 订单取消
    /// </summary>
    Cancel,

    /// <summary>
    /// 失效订单
    /// </summary>
    Expired,
}