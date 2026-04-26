using HaoKao.ContinuationService.Domain.ContinuationSetupModule;

namespace HaoKao.ContinuationService.Application.ContinuationSetupModule.ViewModels;

[AutoMapTo(typeof(CreateContinuationSetupCommand))]
public record CreateContinuationSetupViewModel : IDto
{
    /// <summary>
    /// 续读申请开始时间
    /// </summary>
    [DisplayName("续读申请开始时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime StartTime { get; init; }

    /// <summary>
    /// 续读申请结束时间
    /// </summary>
    [DisplayName("续读申请结束时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime EndTime { get; init; }

    /// <summary>
    /// 续读后的到期时间
    /// </summary>
    [DisplayName("续读后的到期时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime ExpiryTime { get; init; }

    /// <summary>
    /// 产品集合
    /// </summary>
    [DisplayName("产品集合")]
    [Required(ErrorMessage = "{0}不能为空")]
    public IReadOnlyList<SetupProduct> Products { get; init; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [DisplayName("是否启用")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool Enable { get; init; }
}