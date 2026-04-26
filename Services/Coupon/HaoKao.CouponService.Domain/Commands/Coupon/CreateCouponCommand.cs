using FluentValidation;
using HaoKao.CouponService.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace HaoKao.CouponService.Domain.Commands.Coupon;

/// <summary>
/// 创建命令
/// </summary>
/// <param name="EndDate">有效期-开始时间</param>
/// <param name="BeginDate">有效期-结束时间</param>
/// <param name="CouponCode">优惠券卡号</param>
/// <param name="CouponName">优惠券名称</param>
/// <param name="CouponDesc">优惠券说明</param>
/// <param name="PersonName">助教名称</param>
/// <param name="CouponType">优惠券类型 1-抵用券 2-折扣券</param>
/// <param name="ProductIds">适用产品</param>
/// <param name="ProductPackageId">产品包id</param>
/// <param name="ProductName">适用产品</param>
/// <param name="Amount">金额</param>
/// <param name="IsOnlyName">是否实名优惠券 true-实名优惠券 false--非实名优惠券即活动优惠券</param>
/// <param name="Hour">小时</param>
/// <param name="TimeType">时间选择类型</param>
/// <param name="Scope">适用范围类型</param>
/// <param name="ThresholdAmount">适用范围类型</param> 
/// <param name="PersonUserId">助教userid</param>
public record CreateCouponCommand(
    DateTime EndDate,
    DateTime BeginDate,
    string CouponCode,
    string CouponName,
    string CouponDesc,
    string PersonName,
    CouponTypeEnum CouponType,
    List<Guid> ProductIds,
    Guid ProductPackageId,
    string ProductName,
    decimal Amount,
    bool IsOnlyName,
    int Hour,
    TimeTypeEnum TimeType,
    ScopeEnum Scope,
    decimal ThresholdAmount,
    Guid PersonUserId
) : Command("创建")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => CouponName)
                 .NotEmpty().WithMessage("优惠券名称不能为空");
    }
}