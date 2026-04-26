using HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

namespace HaoKao.ContinuationService.Application.ProductExtensionModule.ViewModels;

/// <summary>
/// 可申请服务视图模型
/// </summary>
public record ProductExtensionServiceRequestViewModel : IDto
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 续读策略Id
    /// </summary>
    public Guid PolicyId { get; init; }

    /// <summary>
    /// 剩余申请次数
    /// </summary>
    public int RestOfCount { get; init; }
}

/// <summary>
/// Web端浏览申请详情视图模型
/// </summary>
[AutoMapFrom(typeof(ProductExtensionRequest))]
public record BrowseProductExtensionRequestWebViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 申请理由Id
    /// </summary>
    public Guid ReasonId { get; init; }

    /// <summary>
    /// 详细描述
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// 相关证明
    /// </summary>
    public List<string> Evidences { get; init; }

    /// <summary>
    /// 审核状态
    /// </summary>
    public ProductExtensionRequestState AuditState { get; init; }

    /// <summary>
    /// 审核意见
    /// </summary>
    public string AuditReason { get; init; }

    /// <summary>
    /// 产品过期时间(如果是申请通过，可能是新的过期时间，暂用原过期时间)
    /// </summary>
    public DateTime ExpiryTime { get; init; }

    /// <summary>
    /// 申请时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}

/// <summary>
/// Web端申请列表项视图模型
/// </summary>
[AutoMapFrom(typeof(ProductExtensionRequest))]
public record QueryProductExtensionRequestListWebViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 协议Id
    /// </summary>
    public Guid AgreementId { get; init; }

    /// <summary>
    /// 审核状态
    /// </summary>
    public ProductExtensionRequestState AuditState { get; init; }

    /// <summary>
    /// 申请时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 策略Id
    /// </summary>
    public Guid PolicyId { get; init; }
}