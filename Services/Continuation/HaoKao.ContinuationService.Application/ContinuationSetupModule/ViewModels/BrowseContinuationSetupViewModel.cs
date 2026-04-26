using HaoKao.ContinuationService.Domain.ContinuationSetupModule;

namespace HaoKao.ContinuationService.Application.ContinuationSetupModule.ViewModels;

[AutoMapFrom(typeof(ContinuationSetup))]
public record BrowseContinuationSetupViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 续读申请开始时间
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// 续读申请结束时间
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// 续读后的到期时间
    /// </summary>
    public DateTime ExpiryTime { get; init; }

    /// <summary>
    /// 产品集合
    /// </summary>
    public List<SetupProduct> Products { get; init; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; init; }
}