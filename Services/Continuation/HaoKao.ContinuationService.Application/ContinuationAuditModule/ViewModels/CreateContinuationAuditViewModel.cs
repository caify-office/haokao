using HaoKao.ContinuationService.Domain.ContinuationAuditModule;

namespace HaoKao.ContinuationService.Application.ContinuationAuditModule.ViewModels;

[AutoMapTo(typeof(CreateContinuationAuditCommand))]
public record CreateContinuationAuditViewModel : IDto
{
    /// <summary>
    /// 续读配置Id
    /// </summary>
    [DisplayName("续读配置Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SetupId { get; init; }

    /// <summary>
    /// 产品Id
    /// </summary>
    [DisplayName("产品Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId { get; init; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [DisplayName("产品名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string ProductName { get; init; }

    /// <summary>
    /// 协议Id
    /// </summary>
    [DisplayName("协议Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid AgreementId { get; init; }

    /// <summary>
    /// 协议名称
    /// </summary>
    [DisplayName("协议名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string AgreementName { get; init; }

    /// <summary>
    /// 课程过期时间
    /// </summary>
    [DisplayName("课程过期时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime ExpiryTime { get; init; }

    /// <summary>
    /// 学员姓名
    /// </summary>
    [DisplayName("学员姓名")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string StudentName { get; init; }

    /// <summary>
    /// 续读原因
    /// </summary>
    [DisplayName("续读原因")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Reason { get; init; }

    /// <summary>
    /// 剩余申请次数
    /// </summary>
    [DisplayName("剩余申请次数")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int RestOfCount { get; init; }

    /// <summary>
    /// 详细描述
    /// </summary>
    [DisplayName("详细描述")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string Description { get; init; }

    /// <summary>
    /// 相关证明
    /// </summary>
    [DisplayName("相关证明")]
    [Required(ErrorMessage = "{0}不能为空")]
    public List<string> Evidences { get; init; }

    /// <summary>
    /// 产品的赠品Id集合
    /// </summary>
    [DisplayName("产品的赠品Id集合")]
    public List<string> ProductGifts { get; init; }
}