using HaoKao.ProductService.Domain.Enums;
using HaoKao.ProductService.Domain.Queries;

namespace HaoKao.ProductService.Application.ViewModels.ProductPackage;

[AutoMapFrom(typeof(ProductPackageQuery))]
[AutoMapTo(typeof(ProductPackageQuery))]
public class QueryProductPackageViewModel : QueryDtoBase<ProductPackageQueryListViewModel>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 产品包类型
    /// </summary>
    public ProductType? ProductType { get; set; }

    /// <summary>
    /// 产品Id(返回包含这个产品id的产品包)
    /// </summary>
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 所属年份
    /// </summary>
    public DateTime? Year { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool? Enable { get; set; }

    /// <summary>
    /// 上架
    /// </summary>
    public bool? Shelves { get; set; }
    /// <summary>
    /// 是否排除过期
    /// </summary>
    public bool? IsRemoveExpiry { get; set; }

    /// <summary>
    /// 过滤id数组
    /// </summary>
    public string ExcludeIds { get; set; }

    /// <summary>
    /// 对比
    /// </summary>
    public bool? Contrast { get; set; }

    /// <summary>
    /// 是否体验产品包
    /// </summary>
    public bool? IsExperience { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.ProductPackage))]
[AutoMapTo(typeof(Domain.Entities.ProductPackage))]
public class ProductPackageQueryListViewModel : IDto
{

    /// <summary>
    /// 是否开通
    /// </summary>
   public bool IsOpen { get; set; }

    /// <summary>
    /// 开通时间
    /// </summary>

    public DateTime OpenTime { get; set; }

    public Guid Id { get; set; }

    /// <summary>
    /// 简称
    /// </summary>
    public string SimpleName { get; set; }

    /// <summary>
    /// 售后提醒
    /// </summary>
    public string SalesRemind { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 产品包类型
    /// </summary>
    public ProductType ProductType { get; set; }

    /// <summary>
    /// 产品卡片图片
    /// </summary>
    public string CardImage { get; set; }

    /// <summary>
    /// 详细介绍图片
    /// </summary>
    public string DetailImage { get; set; }

    /// <summary>
    /// 购买人数配置
    /// </summary>
    public int NumberOfBuyers { get; set; }

    /// <summary>
    /// 卖点
    /// </summary>
    public List<string> Selling { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 优惠到期时间
    /// </summary>
    public DateTime PreferentialExpiryTime { get; set; }

    /// <summary>
    /// 所属年份
    /// </summary>
    public DateTime Year { get; set; }

    /// <summary>
    /// 特色服务
    /// </summary>
    public List<Guid> FeaturedService { get; set; }

    /// <summary>
    /// 热门推荐
    /// </summary>
    public bool Hot { get; set; }

    /// <summary>
    /// 对比
    /// </summary>
    public bool Contrast { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 上架
    /// </summary>
    public bool Shelves { get; set; }

    /// <summary>
    /// 对比详细介绍
    /// </summary>
    public Dictionary<Guid, string> Detail { get; set; }

    /// <summary>
    /// 对应的产品列表
    /// </summary>
    public List<Guid> ProductList { get; set; }

    /// <summary>
    /// 讲师列表
    /// </summary>
    public List<Guid> LecturerList { get; set; }

    /// <summary>
    /// 对比字典Id
    /// </summary>
    public Guid? ComparisonDictionaryId { get; set; }

    /// <summary>
    /// 简单描述
    /// </summary>
    public string SimpleDesc { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 是否体验产品包
    /// </summary>
    public bool IsExperience { get; set; }

    /// <summary>
    /// 是否支持花呗分期
    /// </summary>
    public bool IsSupportInstallmentPayment { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int Sort { get; set; }
}