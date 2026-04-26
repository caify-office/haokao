using System;
using System.Collections.Generic;

namespace HaoKao.CouponService.Domain.Queries;

public class PurchaseProductContent
{
    /// <summary>
    /// 内容Id
    /// </summary>
    public Guid ContentId { get; set; }

    /// <summary>
    /// 内容名称
    /// </summary>
    public string ContentName { get; set; }

    /// <summary>
    /// 原价
    /// </summary>
    public decimal ContentAmount { get; set; }

    /// <summary>
    /// 优惠价
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 成交价格
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 苹果内购产品Id
    /// </summary>
    public string AppleInPurchaseProductId { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 优惠券集合
    /// </summary>
    public List<CouponViewModel> Coupons { get; set; }
}

public class CouponViewModel
{
    /// <summary>
    /// 优惠券id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 优惠券名称
    /// </summary>
    public string CouponName { get; set; }

    /// <summary>
    /// 优惠券类型
    /// </summary>
    public int CouponType { get; set; }

    /// <summary>
    /// 折扣/金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 金额加密
    /// </summary>
    public string AmountEncryption { get; set; }
}