namespace HaoKao.Common.Events.Coupon;

/// <summary>
/// 支付成功更新使用状态
/// </summary>
/// <param name="OrderId">订单id</param>
/// <param name="OrderNo">订单编号</param>
/// <param name="ProductName">产品名称</param>
/// <param name="ProductId">产品名称</param>
/// <param name="Amount">订单金额</param>
/// <param name="FactAmount">实际支付金额</param>
/// <param name="PayTime">支付时间</param>
/// <param name="ProductContent">产品内容</param>
public record UpdateIsUseEvent(Guid OrderId, string OrderNo, string ProductName, Guid ProductId, decimal Amount, decimal FactAmount, DateTime PayTime, string ProductContent) : IntegrationEvent;