using FluentValidation;
using HaoKao.CouponService.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace HaoKao.CouponService.Domain.Commands.Coupon;

/// <summary>
/// 更新命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="EndDate">有效期-开始时间</param>
/// <param name="BeginDate">有效期-结束时间</param>
/// <param name="CouponCode">优惠券卡号</param>
/// <param name="CouponName">优惠券名称</param>
/// <param name="CouponDesc">优惠券说明</param>
/// <param name="PersonName">助教名称</param>
/// <param name="CouponType">优惠券类型 1-抵用券 2-折扣券</param>
/// <param name="ProductIds">适用产品:通过产品包-课程逐级筛选，支持多选和反选。不选产品时，该租户下全场产品通用选择产品集合</param>
/// <param name="ProductName">产品包id</param>
/// <param name="ProductPackageId">产品名称</param>
/// <param name="Amount">金额</param>
/// <param name="IsOnlyName">是否实名优惠券 true-实名优惠券 false--非实名优惠券即活动优惠券</param>
/// <param name="Hour">小时</param>
/// <param name="TimeType">时间选择类型</param>
/// <param name="Scope">使用范围</param>
/// <param name="ThresholdAmount">使用范围</param>
/// <param name="PersonUserId">助教userid</param>
public record UpdateCouponCommand(
    Guid Id,
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
) : Command("更新")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => CouponCode)
                 .NotEmpty().WithMessage("优惠券卡号不能为空");

        validator.RuleFor(x => CouponName)
                 .NotEmpty().WithMessage("优惠券名称不能为空");
    }
}