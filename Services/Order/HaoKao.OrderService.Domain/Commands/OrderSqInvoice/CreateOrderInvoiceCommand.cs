using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Domain.Commands.OrderSqInvoice;

/// <summary>
/// 创建订单发票申请命令
/// </summary>
/// <param name="OrderId">订单Id</param>
/// <param name="InvoiceType">发票类型</param>
/// <param name="VatInvoiceType">增值税发票类型</param>
/// <param name="InvoiceTitle">发票抬头</param>
/// <param name="TaxNo">税号</param>
/// <param name="RegistryAddress">注册地址</param>
/// <param name="RegistryTel">注册电话</param>
/// <param name="BankName">开户银行</param>
/// <param name="BankAccount">银行账号</param>
/// <param name="InvoiceFormat">发票形式(获取方式)</param>
/// <param name="Email">电子邮箱</param>
/// <param name="ShippingArea">所在地区</param>
/// <param name="ShippingAddress">收件人地址</param>
/// <param name="ReceiverName">收件人姓名</param>
/// <param name="ReceiverPhone">收件人联系电话</param>
/// <param name="Remark">发票备注</param>
public record CreateOrderInvoiceCommand(
    Guid OrderId,
    InvoiceType InvoiceType,
    VatInvoiceType VatInvoiceType,
    string InvoiceTitle,
    string TaxNo,
    string RegistryAddress,
    string RegistryTel,
    string BankName,
    string BankAccount,
    InvoiceFormat InvoiceFormat,
    string Email,
    string ShippingArea,
    string ShippingAddress,
    string ReceiverName,
    string ReceiverPhone,
    string Remark
) : Command("创建订单发票申请")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => InvoiceTitle).NotEmpty().WithMessage("发票抬头不能为空");
        validator.RuleFor(x => InvoiceFormat).Must(x => x == InvoiceFormat.Electronic).WithMessage("暂不支持纸质发票");
    }
}