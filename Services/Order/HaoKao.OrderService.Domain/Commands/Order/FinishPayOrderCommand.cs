using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Domain.Commands.Order;

/// <summary>
/// 完成支付订单命令
/// </summary>
/// <param name="Id">主键Id</param>
/// <param name="PlatformPayerId">使用的平台配置的支付者的Id</param>
/// <param name="PlatformPayerName">使用的平台配置的支付者的名称</param>
/// <param name="OrderSerialNumber">订单流水号</param>
/// <param name="OrderNumber">订单号</param>
/// <param name="OrderState">订单状态</param>
/// <param name="IosRestorePurchase">苹果内购是否恢复购买过来的</param>
public record FinishPayOrderCommand(
    Guid Id,
    Guid PlatformPayerId,
    string PlatformPayerName,
    string OrderSerialNumber,
    string OrderNumber,
    OrderState OrderState,
    bool IosRestorePurchase
) : Command("完成支付，更新订单状态信息")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => PlatformPayerName)
                 .NotEmpty().WithMessage("使用的平台配置的支付者的名称不能为空")
                 .MaximumLength(50).WithMessage("使用的平台配置的支付者的名称长度不能大于50")
                 .MinimumLength(2).WithMessage("使用的平台配置的支付者的名称长度不能小于2");

        validator.RuleFor(x => OrderNumber)
                 .NotEmpty().WithMessage("订单号不能为空")
                 .MaximumLength(50).WithMessage("订单号长度不能大于50")
                 .MinimumLength(2).WithMessage("订单号长度不能小于2");
    }
}