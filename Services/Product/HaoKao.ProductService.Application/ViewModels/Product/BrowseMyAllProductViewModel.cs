using HaoKao.Common.Enums;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.ViewModels.Product;

[AutoMapFrom(typeof(Domain.Entities.Product))]
[AutoMapTo(typeof(Domain.Entities.Product))]
public class BrowseMyAllProductViewModel : IDto
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
    /// 0-按日期 1-按天数
    /// </summary>
    public ExpiryTimeTypeEnum ExpiryTimeTypeEnum { get; set; }

    /// <summary>
    /// 按天数
    /// </summary>
    public int Days { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public string Year { get; set; }

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
    public ICollection<AssistantProductPermission> AssistantProductPermissions { get; set; } = new List<AssistantProductPermission>();

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


}
