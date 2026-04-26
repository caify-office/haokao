using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveCoupon;

/// <summary>
/// 批量更新直播优惠卷命令
/// </summary>
public record UpdateLiveCouponCommand(
    List<UpdateLiveCouponModel> models
) : Command("批量更新直播优惠卷");

/// <param name="Id"></param>
/// <param name="LiveVideoId">所属视频直播Id</param>
/// <param name="LiveCouponId">优惠卷Id</param>
/// <param name="LiveCouponName">优惠卷名称</param>
/// <param name="Amount">金额/折扣--合并一个字段  折扣85折显示0.85</param>
/// <param name="CouponType">优惠券类型 1-抵用券 2-折扣券</param>
/// <param name="Scope">适用范围</param>
/// <param name="IsShelves">是否上架</param>
/// <param name="Sort">排序</param>
public record UpdateLiveCouponModel(
    Guid Id,
    Guid LiveVideoId,
    Guid LiveCouponId,
    string LiveCouponName,
    decimal Amount,
    CouponTypeEnum CouponType,
    ScopeEnum Scope,
    bool IsShelves,
    int Sort
);