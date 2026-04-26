using System;

namespace HaoKao.CouponService.Domain.Models;

public class UserCouponPerformance : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable, IIncludeCreatorId<Guid>, IIncludeCreatorName
{
    /// <summary>
    /// 订单编号
    /// </summary>
    public string OrderNo { get; set; }

    /// <summary>
    /// 订单id
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// 手机号码--冗余
    /// </summary>
    public string TelPhone { get; set; }

    /// <summary>
    ///昵称--冗余
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    ///产品名称--冗余
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    ///实际支付金额--冗余
    /// </summary>
    public decimal FactAmount { get; set; }

    /// <summary>
    ///产品原价--冗余
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    ///支付时间--冗余
    /// </summary>
    public DateTime PayTime { get; set; }

    /// <summary>
    ///备注--后台手动添加的默认手动添加
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 助教实名名称
    /// </summary>
    public string PersonName { get; set; }

    /// <summary>
    /// 营销助教userid
    /// </summary>
    public Guid PersonUserId { get; set; }

    /// <summary>
    ///优惠券关联使用的userId
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 优惠券关联使用的userName
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户
    /// </summary>
    public Guid TenantId { get; set; }
}