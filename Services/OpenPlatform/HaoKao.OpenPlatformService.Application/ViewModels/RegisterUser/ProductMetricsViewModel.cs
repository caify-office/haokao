using System.Text.Json.Serialization;

namespace HaoKao.OpenPlatformService.Application.ViewModels.RegisterUser;

public class ProductMetricsQueryViewModel : IDto
{
    [JsonIgnore]
    public DateTime? Start { get; set; }

    [JsonIgnore]
    public DateTime? End { get; set; }
}

/// <summary>
/// 每日趋势查询
/// </summary>
public class DailyTrendQueryViewModel : ProductMetricsQueryViewModel
{
    /// <summary>
    /// 上一个7天的起始日期
    /// </summary>
    public DateTime? PrevDate { get; set; }

    /// <summary>
    /// 下一个7天的起始日期
    /// </summary>
    public DateTime? NextDate { get; set; }
}

/// <summary>
/// 每日趋势
/// </summary>
public class DailyTrendListViewModel : IDto
{
    public string Date { get; set; }

    public int Count { get; set; }
}

/// <summary>
/// 各端占比
/// </summary>
public class ClientGroupingListViewModel : IDto
{
    public string ClientId { get; set; }

    public int Count { get; set; }
}