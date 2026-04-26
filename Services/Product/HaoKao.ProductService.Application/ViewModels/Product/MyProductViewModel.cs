using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.ViewModels.Product;

[AutoMapFrom(typeof(Domain.Entities.Product))]
[AutoMapTo(typeof(Domain.Entities.Product))]
public class MyProductViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// 预约起始人数（只有直播类型的产品需要用到）
    /// </summary>
    public int ReservationBaseNumber { get; set; }

    /// <summary>
    /// 产品类别
    /// </summary>
    public ProductType ProductType { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 是否上架
    /// </summary>
    public bool IsShelves { get; set; }

    /// <summary>
    /// 产品描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 产品图片
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 产品详情图片
    /// </summary>
    public string DetailImage { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// 优惠价格
    /// </summary>
    public double DiscountedPrice { get; set; }

    /// <summary>
    /// 苹果内购产品ID
    /// </summary>
    public string AppleProductId { get; set; }

    /// <summary>
    /// 答疑
    /// </summary>
    public bool Answering { get; set; }

    /// <summary>
    /// 对应的权限列表
    /// </summary>
    public ICollection<BrowseProductPermissionViewModel> ProductPermissions { get; set; } =
        new List<BrowseProductPermissionViewModel>();

    /// <summary>
    /// 对应的智辅权限列表
    /// </summary>
    public ICollection<BrowseAssistantProductPermissionViewModel> AssistantProductPermissions { get; set; } =
        new List<BrowseAssistantProductPermissionViewModel>();

    /// <summary>
    /// 产品协议
    /// </summary>
    public Guid? Agreement { get; set; }

    /// <summary>
    /// 赠送列表
    /// </summary>
    public Dictionary<Guid, string> GiveAwayAList { get; set; }

    /// <summary>
    /// 是否体验产品
    /// </summary>
    public bool IsExperience { get; set; }

    /// <summary>
    /// 我的过期时间
    /// </summary>
    public DateTime MyExpireTime { get; set; }

    /// <summary>
    /// 我的最早购买时间
    /// </summary>
    public DateTime MyEarliestBuyTime { get; set; }

    /// <summary>
    /// 当前产品是否过期
    /// </summary>
    public bool IsExpire => DateTime.Now > MyExpireTime;
}