using FluentValidation;
using System;

namespace HaoKao.CouponService.Domain.Commands.UserCouponPerformance;

/// <summary>
/// 创建命令
/// </summary>
/// <param name="Id"></param>
/// <param name="OrderNo">订单编号</param>
/// <param name="OrderId">订单id</param>
/// <param name="TelPhone">手机号码--冗余</param>
/// <param name="NickName">昵称--冗余</param>
/// <param name="ProductName">产品名称--冗余</param>
/// <param name="FactAmount">实际支付金额--冗余</param>
/// <param name="Amount">产品原价--冗余</param>
/// <param name="PayTime">支付时间--冗余</param>
/// <param name="Remark">备注--后台手动添加的默认手动添加</param>
/// <param name="PersonName">销售人员名称</param>
/// <param name="PersonUserId">备注--后台手动添加的默认手动添加</param>
public record CreateUserCouponPerformanceCommand(
    Guid Id,
    string OrderNo,
    Guid OrderId,
    string TelPhone,
    string NickName,
    string ProductName,
    decimal FactAmount,
    decimal Amount,
    DateTime PayTime,
    string Remark,
    string PersonName,
    Guid PersonUserId
) : Command("创建")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator) { }
}