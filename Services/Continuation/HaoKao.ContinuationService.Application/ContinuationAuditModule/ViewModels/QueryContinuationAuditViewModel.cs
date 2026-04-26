using HaoKao.ContinuationService.Domain.ContinuationAuditModule;

namespace HaoKao.ContinuationService.Application.ContinuationAuditModule.ViewModels;

[AutoMapFrom(typeof(ContinuationAuditQuery))]
[AutoMapTo(typeof(ContinuationAuditQuery))]
public class QueryContinuationAuditViewModel : QueryDtoBase<QueryContinuationAuditListViewModel>
{
    /// <summary>
    /// 产品名称
    /// </summary>
    [JsonIgnore]
    public string ProductName { get; set; }

    /// <summary>
    /// 学员姓名
    /// </summary>
    [JsonIgnore]
    public string StudentName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [JsonIgnore]
    public AuditState? AuditState { get; set; }
}

[AutoMapFrom(typeof(ContinuationAudit))]
[AutoMapTo(typeof(ContinuationAudit))]
public record QueryContinuationAuditListViewModel : IDto
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
    /// 产品名称
    /// </summary>
    public string ProductName { get; init; }

    /// <summary>
    /// 产品过期时间
    /// </summary>
    public DateTime ExpiryTime { get; init; }

    /// <summary>
    /// 学员姓名
    /// </summary>
    public string StudentName { get; init; }

    /// <summary>
    /// 续读原因
    /// </summary>
    public Guid Reason { get; init; }

    /// <summary>
    /// 审核状态
    /// </summary>
    public AuditState AuditState { get; init; }

    /// <summary>
    /// 不通过原因
    /// </summary>
    public string AuditReason { get; init; }

    /// <summary>
    /// 申请人Id
    /// </summary>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// 申请时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}