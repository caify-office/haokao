using HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

namespace HaoKao.ContinuationService.Application.ProductExtensionRequestModule.ViewModels;

/// <summary>
/// 续读申请审核日志视图模型
/// </summary>
[AutoMapFrom(typeof(ProductExtensionAuditLog))]
public record ProductExtensionAuditLogViewModel : IDto
{
    /// <summary>
    /// 实体Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 关联的申请Id
    /// </summary>
    public Guid RequestId { get; set; }

    /// <summary>
    /// 变更后的状态
    /// </summary>
    public ProductExtensionRequestState NewState { get; set; }

    /// <summary>
    /// 审核意见/备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 操作人Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 操作人名称 (冗余字段，便于历史追溯)
    /// </summary>
    public string CreatorName { get; set; }
}

/// <summary>
/// 创建课程续读申请视图模型
/// </summary>
[AutoMapTo(typeof(CreateProductExtensionRequestCommand))]
public record CreateProductExtensionRequestViewModel : IDto
{
    /// <summary>
    /// 策略Id
    /// </summary>
    [DisplayName("策略Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid PolicyId { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    [DisplayName("产品Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [DisplayName("产品名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string ProductName { get; set; }

    /// <summary>
    /// 协议Id
    /// </summary>
    [DisplayName("协议Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid AgreementId { get; set; }

    /// <summary>
    /// 协议名称
    /// </summary>
    [DisplayName("协议名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string AgreementName { get; set; }

    /// <summary>
    /// 产品过期时间
    /// </summary>
    [DisplayName("产品过期时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 学员姓名
    /// </summary>
    [DisplayName("学员姓名")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string StudentName { get; set; }

    /// <summary>
    /// 申请理由Id
    /// </summary>
    [DisplayName("申请理由Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ReasonId { get; set; }

    /// <summary>
    /// 剩余申请次数
    /// </summary>
    [DisplayName("剩余申请次数")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int RestOfCount { get; set; }

    /// <summary>
    /// 详细描述
    /// </summary>
    [DisplayName("详细描述")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string Description { get; set; }

    /// <summary>
    /// 相关证明
    /// </summary>
    [DisplayName("相关证明")]
    [Required(ErrorMessage = "{0}不能为空")]
    public List<string> Evidences { get; set; }

    /// <summary>
    /// 产品的赠品Id集合
    /// </summary>
    [DisplayName("产品的赠品Id集合")]
    public List<string> ProductGifts { get; set; }
}

/// <summary>
/// 更新课程续读申请状态视图模型 (审核)
/// </summary>
[AutoMapTo(typeof(UpdateProductExtensionRequestStateCommand))]
public record UpdateProductExtensionRequestStateViewModel : IDto
{
    /// <summary>
    /// 申请Id
    /// </summary>
    [DisplayName("申请Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; set; }

    /// <summary>
    /// 审核状态
    /// </summary>
    [DisplayName("审核状态")]
    [Required(ErrorMessage = "{0}不能为空")]
    public ProductExtensionRequestState State { get; set; }

    /// <summary>
    /// 审核意见/理由
    /// </summary>
    [DisplayName("审核意见")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string Remark { get; set; }
}

/// <summary>
/// 浏览课程续读申请视图模型
/// </summary>
[AutoMapFrom(typeof(ProductExtensionRequest))]
public record BrowseProductExtensionRequestViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// 策略Id
    /// </summary>
    public Guid PolicyId { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 协议Id
    /// </summary>
    public Guid AgreementId { get; set; }

    /// <summary>
    /// 协议名称
    /// </summary>
    public string AgreementName { get; set; }

    /// <summary>
    /// 产品过期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 学员姓名
    /// </summary>
    public string StudentName { get; set; }

    /// <summary>
    /// 申请理由Id
    /// </summary>
    public Guid ReasonId { get; set; }

    /// <summary>
    /// 剩余申请次数
    /// </summary>
    public int RestOfCount { get; set; }

    /// <summary>
    /// 详细描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 相关证明
    /// </summary>
    public List<string> Evidences { get; set; }

    /// <summary>
    /// 产品的赠品Id集合
    /// </summary>
    public List<string> ProductGifts { get; set; }

    /// <summary>
    /// 当前审核状态
    /// </summary>
    public ProductExtensionRequestState AuditState { get; set; }

    /// <summary>
    /// 当前审核意见
    /// </summary>
    public string AuditReason { get; set; }

    /// <summary>
    /// 最后审核时间
    /// </summary>
    public DateTime? AuditTime { get; set; }

    /// <summary>
    /// 最后审核人
    /// </summary>
    public string AuditOperatorName { get; set; }

    /// <summary>
    /// 申请时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 申请人Id
    /// </summary>
    public Guid CreatorId { get; set; }
    
    /// <summary>
    /// 审核日志集合
    /// </summary>
    public List<ProductExtensionAuditLogViewModel> AuditLogs { get; set; }
}

/// <summary>
/// 查询课程续读申请视图模型
/// </summary>
[AutoMapFrom(typeof(ProductExtensionRequestQuery))]
[AutoMapTo(typeof(ProductExtensionRequestQuery))]
public class QueryProductExtensionRequestViewModel : QueryDtoBase<QueryProductExtensionRequestListViewModel>
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
    public ProductExtensionRequestState? AuditState { get; set; }

    /// <summary>
    /// 申请人Id
    /// </summary>
    [JsonIgnore]
    public Guid? CreatorId { get; set; }
}

/// <summary>
/// 课程续读申请列表视图模型
/// </summary>
[AutoMapFrom(typeof(ProductExtensionRequest))]
public record QueryProductExtensionRequestListViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }
    
    /// <summary>
    /// 协议名称
    /// </summary>
    public string AgreementName { get; set; }

    /// <summary>
    /// 产品过期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 学员姓名
    /// </summary>
    public string StudentName { get; set; }
    
    /// <summary>
    /// 当前审核状态
    /// </summary>
    public ProductExtensionRequestState AuditState { get; set; }

    /// <summary>
    /// 当前审核意见
    /// </summary>
    public string AuditReason { get; set; }

    /// <summary>
    /// 申请时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    
    /// <summary>
    /// 申请人Id
    /// </summary>
    public Guid CreatorId { get; set; }
}