using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveCoupon;

/// <summary>
/// 创建直播产品包命令
/// </summary>
public record CreateLiveCouponCommand(
    List<CreateLiveCouponModel> models
) : Command("批量创建直播优惠卷");

/// <param name="LiveVideoId">所属视频直播Id</param>
/// <param name="LiveCouponId">优惠卷Id</param>
/// <param name="LiveCouponName">优惠卷名称</param>
/// <param name="Amount">金额/折扣--合并一个字段  折扣85折显示0.85</param>
/// <param name="CouponType">优惠券类型 1-抵用券 2-折扣券</param>
/// <param name="Scope">适用范围</param>
/// <param name="IsShelves">是否上架</param>
/// <param name="Sort">排序</param>
public record CreateLiveCouponModel(
    Guid LiveVideoId,
    Guid LiveCouponId,
    string LiveCouponName,
    decimal Amount,
    CouponTypeEnum CouponType,
    ScopeEnum Scope,
    bool IsShelves,
    int Sort
);