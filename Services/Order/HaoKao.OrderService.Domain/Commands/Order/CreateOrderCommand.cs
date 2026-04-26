using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Domain.Commands.Order;

/// <summary>
/// 创建订单表命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="OrderSerialNumber">订单流水号</param>
/// <param name="PurchaseProductId">购买产品ID</param>
/// <param name="PurchaseName">购买产品名称</param>
/// <param name="PurchaseProductType">产品类型</param>
/// <param name="OrderAmount">订单金额</param>
/// <param name="ActualAmount">实际金额</param>
/// <param name="Phone">手机号</param>
/// <param name="LiveId">直播Id</param>
/// <param name="ProductContents">购买产品内容详情</param>
/// <param name="CreatorId">下单用户Id</param>
/// <param name="CreatorName">下单用户名称</param>
/// <param name="CreateTime">下单时间</param>
/// <param name="UpdateTime">支付时间</param>
/// <param name="ClientId">客户Id</param>
/// <param name="ClientName">客户名称</param>
public record CreateOrderCommand(
    Guid Id,
    string OrderSerialNumber,
    Guid PurchaseProductId,
    string PurchaseName,
    PurchaseProductType PurchaseProductType,
    decimal OrderAmount,
    decimal ActualAmount,
    string Phone,
    Guid? LiveId,
    List<PurchaseProductContent> ProductContents,
    Guid CreatorId,
    string CreatorName,
    DateTime? CreateTime,
    DateTime? UpdateTime,
    string ClientId,
    string ClientName
) : Command("创建订单表")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => PurchaseName)
                 .NotEmpty().WithMessage("购买产品名称不能为空")
                 .MaximumLength(50).WithMessage("购买产品名称长度不能大于50")
                 .MinimumLength(2).WithMessage("购买产品名称长度不能小于2");
    }
}