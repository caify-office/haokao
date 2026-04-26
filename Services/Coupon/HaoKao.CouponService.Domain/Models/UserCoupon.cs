using HaoKao.CouponService.Domain.Enumerations;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace HaoKao.CouponService.Domain.Models;

public class UserCoupon : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable, IIncludeCreatorId<Guid>, IIncludeCreatorName
{
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public Coupon Coupon { get; set; }

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

    public Guid ProductId { get; set; }

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
    /// 优惠券id
    /// </summary>
    public Guid CouponId { get; set; }

    /// <summary>
    /// 是否使用 0-未使用 1-已使用
    /// </summary>
    public bool IsUse { get; set; }

    /// <summary>
    /// 是否锁定
    /// </summary>
    public bool IsLock { get; set; }

    /// <summary>
    /// 渠道类型
    /// </summary>
    public ChannelType ChannelType { get; set; }

    /// <summary>
    /// 是否已通知
    /// </summary>
    public bool Notified { get; set; }

    /// <summary>
    /// 微信用户的OpenId
    /// </summary>
    public string OpenId { get; set; }

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
    /// 开始时间（可为创建时间，可为优惠卷有效期开始时间）
    /// </summary>
    public DateTime BeginTime { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 租户
    /// </summary>
    public Guid TenantId { get; set; }
}