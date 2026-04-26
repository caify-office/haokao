using Girvs.BusinessBasis.Queries;
using HaoKao.ProductService.Domain.Enums;
using HaoKao.ProductService.Domain.Queries;

namespace HaoKao.ProductService.Application.ViewModels.Product;

[AutoMapFrom(typeof(ProductQuery))]
[AutoMapTo(typeof(ProductQuery))]
public class QueryProductViewModel : QueryDtoBase<ProductListViewModel>
{
    /// <summary>
    /// 产品名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 产品类别
    /// </summary>
    public ProductType? ProductType { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public string Year { get; set; }


    /// <summary>
    /// 启用
    /// </summary>
    [QueryCacheKey]
    public bool? Enable { get; set; }

    /// <summary>
    /// 是否上架
    /// </summary>
    public bool? IsShelves { get; set; }

    /// <summary>
    /// 过滤id数组
    /// </summary>
    public string ExcludeIds { get; set; }

    /// <summary>
    /// 是否需要签署协议
    /// </summary>
    public bool? IsNeedAgreement { get; set; }

    /// <summary>
    /// 是否过期 (true:返回已过期优惠卷，false：返回未过期优惠卷，不传：过期和未过期的都返回)
    /// </summary>
    public bool? IsExpired { get; set; }

    /// <summary>
    /// 是否体验产品
    /// </summary>
    public bool? IsExperience { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.Product))]
[AutoMapTo(typeof(Domain.Entities.Product))]
public class ProductListViewModel : IDto
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

[AutoMapFrom(typeof(Domain.Entities.Product))]
[AutoMapTo(typeof(Domain.Entities.Product))]
public class SimpleProductListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// 优惠价格
    /// </summary>
    public double DiscountedPrice { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.Product))]
[AutoMapTo(typeof(Domain.Entities.Product))]
public class RelatedProductListViewModel : IDto
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
}