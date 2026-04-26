using HaoKao.ContinuationService.Domain.ContinuationSetupModule;

namespace HaoKao.ContinuationService.Application.ContinuationSetupModule.ViewModels;

[AutoMapFrom(typeof(ContinuationSetupQuery))]
[AutoMapTo(typeof(ContinuationSetupQuery))]
public class QueryContinuationSetupViewModel : QueryDtoBase<QueryContinuationSetupListViewModel>
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? Enable { get; set; }
}

[AutoMapFrom(typeof(ContinuationSetup))]
[AutoMapTo(typeof(ContinuationSetup))]
public record QueryContinuationSetupListViewModel : IDto
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
    [JsonIgnore]
    public IReadOnlyList<SetupProduct> Products { get; init; }

    /// <summary>
    /// 产品Ids
    /// </summary>
    public IReadOnlyList<Guid> ProductIds => Products?.Select(x => x.ProductId).ToList();

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [JsonIgnore]
    public DateTime CreateTime { get; init; }
}