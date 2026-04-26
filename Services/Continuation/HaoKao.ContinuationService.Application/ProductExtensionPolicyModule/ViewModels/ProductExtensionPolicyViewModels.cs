using HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

namespace HaoKao.ContinuationService.Application.ProductExtensionPolicyModule.ViewModels;

/// <summary>
/// 产品策略信息
/// </summary>
[AutoMapTo(typeof(PolicyProduct))]
[AutoMapFrom(typeof(PolicyProduct))]
public record PolicyProductViewModel : IDto
{
    /// <summary>
    /// 产品Id
    /// </summary>
    [DisplayName("产品Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId { get; init; }

    /// <summary>
    /// 协议Id
    /// </summary>
    [DisplayName("协议Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid AgreementId { get; init; }

    /// <summary>
    /// 允许续读的次数限制 (0表示不限)
    /// </summary>
    [DisplayName("允许续读的次数限制")]
    public int MaxExtensionCount { get; init; }
}

/// <summary>
/// 创建课程续读策略视图模型
/// </summary>
[AutoMapTo(typeof(CreateProductExtensionPolicyCommand))]
public record CreateProductExtensionPolicyViewModel : IDto
{
    /// <summary>
    /// 策略名称
    /// </summary>
    [DisplayName("策略名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; init; }

    /// <summary>
    /// 申请开始时间
    /// </summary>
    [DisplayName("申请开始时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime StartTime { get; init; }

    /// <summary>
    /// 申请结束时间
    /// </summary>
    [DisplayName("申请结束时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime EndTime { get; init; }

    /// <summary>
    /// 延期类型
    /// </summary>
    [DisplayName("延期类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public ExtensionType ExtensionType { get; init; }

    /// <summary>
    /// 延长天数 (当 ExtensionType 为 Duration 时使用)
    /// </summary>
    [DisplayName("延长天数")]
    public int? ExtensionDays { get; init; }

    /// <summary>
    /// 固定的过期时间 (当 ExtensionType 为 FixedDate 时使用)
    /// </summary>
    [DisplayName("固定的过期时间")]
    public DateTime? ExpiryDate { get; init; }

    /// <summary>
    /// 产品集合
    /// </summary>
    [DisplayName("产品集合")]
    [Required(ErrorMessage = "{0}不能为空")]
    public List<PolicyProductViewModel> Products { get; init; } = [];

    /// <summary>
    /// 是否启用
    /// </summary>
    [DisplayName("是否启用")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsEnable { get; init; }
}

/// <summary>
/// 更新课程续读策略视图模型
/// </summary>
[AutoMapTo(typeof(UpdateProductExtensionPolicyCommand))]
public record UpdateProductExtensionPolicyViewModel : CreateProductExtensionPolicyViewModel
{
    /// <summary>
    /// 主键
    /// </summary>
    [DisplayName("主键")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; init; }
}

/// <summary>
/// 浏览课程续读策略视图模型
/// </summary>
[AutoMapFrom(typeof(ProductExtensionPolicy))]
public record BrowseProductExtensionPolicyViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// 策略名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 申请开始时间
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// 申请结束时间
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// 延期类型
    /// </summary>
    public ExtensionType ExtensionType { get; init; }

    /// <summary>
    /// 延长天数
    /// </summary>
    public int? ExtensionDays { get; init; }

    /// <summary>
    /// 固定的过期时间
    /// </summary>
    public DateTime? ExpiryDate { get; init; }

    /// <summary>
    /// 产品集合
    /// </summary>
    public List<PolicyProductViewModel> Products { get; init; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnable { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }
    
    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; init; }
}

/// <summary>
/// 查询课程续读策略视图模型
/// </summary>
[AutoMapFrom(typeof(ProductExtensionPolicyQuery))]
[AutoMapTo(typeof(ProductExtensionPolicyQuery))]
public class QueryProductExtensionPolicyViewModel : QueryDtoBase<QueryProductExtensionPolicyListViewModel>
{
    /// <summary>
    /// 策略名称
    /// </summary>
    [JsonIgnore]
    public string Name { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [JsonIgnore]
    public bool? IsEnable { get; set; }
}

/// <summary>
/// 课程续读策略列表视图模型
/// </summary>
[AutoMapFrom(typeof(ProductExtensionPolicy))]
public record QueryProductExtensionPolicyListViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// 策略名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 申请开始时间
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// 申请结束时间
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// 延期类型
    /// </summary>
    public ExtensionType ExtensionType { get; init; }

    /// <summary>
    /// 延长天数
    /// </summary>
    public int? ExtensionDays { get; init; }

    /// <summary>
    /// 固定的过期时间
    /// </summary>
    public DateTime? ExpiryDate { get; init; }
    
    /// <summary>
    /// 产品集合
    /// </summary>
    public List<PolicyProductViewModel> Products { get; init; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnable { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}