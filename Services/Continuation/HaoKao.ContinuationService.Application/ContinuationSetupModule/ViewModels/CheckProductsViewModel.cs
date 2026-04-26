namespace HaoKao.ContinuationService.Application.ContinuationSetupModule.ViewModels;

public record CheckProductsViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; init; }

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
    /// 产品Id集合
    /// </summary>
    [DisplayName("产品Id集合")]
    [Required(ErrorMessage = "{0}不能为空")]
    public IReadOnlyList<Guid> ProductIds { get; init; }
}