namespace HaoKao.OrderService.Domain.ValueObjects;

/// <summary>
/// 平台产品
/// </summary>
public record Product
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; init; }

    /// <summary>
    /// 产品类别
    /// </summary>
    public int ProductType { get; init; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; init; }

    /// <summary>
    /// 是否上架
    /// </summary>
    public bool IsShelves { get; init; }

    /// <summary>
    /// 产品描述
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// 产品图片
    /// </summary>
    public string Icon { get; init; }

    /// <summary>
    /// 产品详情图片
    /// </summary>
    public string DetailImage { get; init; }

    /// <summary>
    /// 0-按日期 1-按天数
    /// </summary>
    public int ExpiryTimeTypeEnum { get; init; }

    /// <summary>
    /// 按天数
    /// </summary>
    public int Days { get; init; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime ExpiryTime { get; init; }

    /// <summary>
    /// 所属年份
    /// </summary>
    public string Year { get; init; }

    /// <summary>
    /// 价格
    /// </summary>
    public double Price { get; init; }

    /// <summary>
    /// 优惠价格
    /// </summary>
    public double DiscountedPrice { get; init; }

    /// <summary>
    /// 苹果内购产品ID
    /// </summary>
    public string AppleProductId { get; init; }

    /// <summary>
    /// 答疑
    /// </summary>
    public bool Answering { get; init; }

    /// <summary>
    /// 产品协议
    /// </summary>
    public Guid? Agreement { get; init; }

    /// <summary>
    /// 赠送列表
    /// </summary>
    public Dictionary<Guid, string> GiveAwayAList { get; init; }

    /// <summary>
    /// 是否体验产品
    /// </summary>
    public bool IsExperience { get; init; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; init; }

    /// <summary>
    /// 创建者
    /// </summary>
    public string CreatorName { get; init; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}