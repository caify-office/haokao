using FluentValidation;
using HaoKao.CouponService.Domain.Enumerations;
using System;

namespace HaoKao.CouponService.Domain.Commands.UserCoupon;

/// <summary>
/// 创建命令
/// </summary>
/// <param name="CouponId">优惠券id</param>
/// <param name="IsUse">是否使用 0-未使用 1-已使用</param>
/// <param name="OrderNo">订单编号</param>
/// <param name="OrderId">订单id</param>
/// <param name="FactAmount">实际支付金额</param>
/// <param name="NickName">昵称</param>
/// <param name="ProductName">产品名称</param>
/// <param name="TelPhone">手机号码</param>
/// <param name="Remark">备注</param>
/// <param name="ChannelType"></param>
/// <param name="OpenId"></param>
public record CreateUserCouponCommand(
    string CouponId,
    bool IsUse,
    string OrderNo,
    Guid OrderId,
    decimal FactAmount,
    string NickName,
    string ProductName,
    string TelPhone,
    string Remark,
    ChannelType ChannelType,
    string OpenId) : Command("创建")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator) { }
}