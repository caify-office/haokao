using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.ViewModels.ProductPackage;

[AutoMapTo(typeof(Domain.Commands.ProductPackage.UpdateProductPackageCommand))]
public class UpdateProductPackageViewModel : IDto
{
    public Guid  Id { get; set; }
    /// <summary>
    /// 简称
    /// </summary>
    [DisplayName("简称")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string SimpleName { get; set; }

    /// <summary>
    /// 售后提醒
    /// </summary>
    [DisplayName("简称")]
    public string SalesRemind { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    [DisplayName("名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 产品包类型
    /// </summary>
    [DisplayName("产品包类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public ProductType ProductType { get; set; }

    /// <summary>
    /// 产品卡片图片
    /// </summary>
    [DisplayName("产品卡片图片")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(1000, ErrorMessage = "{0}长度不能大于{1}")]
    public string CardImage { get; set; }

    /// <summary>
    /// 详细介绍图片
    /// </summary>
    [DisplayName("详细介绍图片")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(1000, ErrorMessage = "{0}长度不能大于{1}")]
    public string DetailImage { get; set; }


    /// <summary>
    /// 购买人数配置
    /// </summary>
    [DisplayName("购买人数配置")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int NumberOfBuyers { get; set; }

    /// <summary>
    /// 卖点
    /// </summary>
    [DisplayName("卖点")]
    public List<string> Selling { get; set; } = [];

    /// <summary>
    /// 到期时间
    /// </summary>
    [DisplayName("到期时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 优惠到期时间
    /// </summary>
    [DisplayName("优惠到期时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime PreferentialExpiryTime { get; set; }

    /// <summary>
    /// 所属年份
    /// </summary>
    [DisplayName("所属年份")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime Year { get; set; }

    /// <summary>
    /// 特色服务
    /// </summary>
    [DisplayName("特色服务")]

    public List<Guid> FeaturedService { get; set; } = [];

    /// <summary>
    /// 热门推荐
    /// </summary>
    [DisplayName("热门推荐")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool Hot { get; set; }

    /// <summary>
    /// 对比
    /// </summary>
    [DisplayName("对比")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool Contrast { get; set; }

    /// <summary>
    /// 对比详细介绍
    /// </summary>
    [DisplayName("对比详细介绍")]
    public Dictionary<Guid, string> Detail { get; set; } = new();

    /// <summary>
    /// 讲师列表
    /// </summary>
    [DisplayName("讲师列表")]
    public List<Guid> LecturerList { get; set; } = [];

    /// <summary>
    /// 对比字典Id
    /// </summary>
    [DisplayName("对比字典Id")]
    public Guid? ComparisonDictionaryId { get; set; }

    /// <summary>
    /// 简单描述
    /// </summary>
    [DisplayName("简单描述")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string SimpleDesc { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [DisplayName("描述")]
    [MaxLength(1000, ErrorMessage = "{0}长度不能大于{1}")]
    public string Desc { get; set; }

    /// <summary>
    /// 是否体验产品包
    /// </summary>
    [DisplayName("是否体验产品包")]
    public bool IsExperience { get; set; }


    /// <summary>
    /// 是否支持花呗分期
    /// </summary>
    [DisplayName("是否支持花呗分期")]
    public bool IsSupportInstallmentPayment { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [DisplayName("排序号")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Sort { get; set; }
}