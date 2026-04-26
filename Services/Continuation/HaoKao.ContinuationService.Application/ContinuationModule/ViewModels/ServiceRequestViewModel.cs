namespace HaoKao.ContinuationService.Application.ContinuationModule.ViewModels;

public record ServiceRequestViewModel : IDto
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 续读配置Id
    /// </summary>
    public Guid SetupId { get; init; }

    /// <summary>
    /// 剩余申请次数
    /// </summary>
    public int RestOfCount { get; init; }
}