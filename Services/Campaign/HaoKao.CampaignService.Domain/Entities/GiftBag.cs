using HaoKao.CampaignService.Domain.Enums;

namespace HaoKao.CampaignService.Domain.Entities;

public sealed class GiftBag : AggregateRoot<Guid>,
                              IIncludeCreateTime,
                              IIncludeUpdateTime,
                              IIncludeMultiTenant<Guid>,
                              ITenantShardingTable
{
    /// <summary>
    /// 活动名称
    /// </summary>
    public string CampaignName { get; init; }

    /// <summary>
    /// 礼包类型
    /// </summary>
    public GiftType GiftType { get; init; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; init; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// 是否发布
    /// </summary>
    public bool IsPublished { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; init; }

    /// <summary>
    /// 领取人数
    /// </summary>
    public int ReceiveCount { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// PC网站图片
    /// </summary>
    public GiftBagImageSet WebSiteImageSet { get; init; }

    /// <summary>
    /// 微信小程序图片
    /// </summary>
    public GiftBagImageSet WeChatMiniProgramImageSet { get; init; }

    /// <summary>
    /// 领取规则
    /// </summary>
    public IReadOnlyList<Guid> ReceiveRules { get; init; }

    /// <summary>
    /// 礼品包领取记录
    /// </summary>
    public ICollection<GiftBagReceiveLog> GiftBagReceiveLogs { get; init; }
}

/// <summary>
/// 礼包图片
/// </summary>
public sealed class GiftBagImageSet(Uri giftIcon, Uri popupImage, Uri receivePage) : ValueObject<GiftBagImageSet>
{
    /// <summary>
    /// 礼包图标
    /// </summary>
    public Uri GiftIcon { get; } = giftIcon;

    /// <summary>
    /// 弹窗图片
    /// </summary>
    public Uri PopupImage { get; } = popupImage;

    /// <summary>
    /// 领取页面
    /// </summary>
    public Uri ReceivePage { get; } = receivePage;

    protected override bool EqualsCore(GiftBagImageSet other)
    {
        if (other == null || other.GetType() != GetType())
        {
            return false;
        }

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    protected override int GetHashCodeCore()
    {
        return GetEqualityComponents()
               .Select(x => x?.GetHashCode() ?? 0)
               .Aggregate((x, y) => x ^ y);
    }

    private IEnumerable<object> GetEqualityComponents()
    {
        yield return GiftIcon;
        yield return PopupImage;
        yield return ReceivePage;
    }
}