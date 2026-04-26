using System.Linq;
using System.Text.Json.Serialization;

namespace HaoKao.DrawPrizeService.Application.ViewModels.DrawPrize;

[AutoMapFrom(typeof(DrawPrizeQuery))]
[AutoMapTo(typeof(DrawPrizeQuery))]
public class DrawPrizeQueryViewModel : QueryDtoBase<DrawPrizeQueryListViewModel>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public DrawPrizeType? DrawPrizeType { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.DrawPrize))]
[AutoMapTo(typeof(Domain.Entities.DrawPrize))]
public class DrawPrizeQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 活动背景图
    /// </summary>
    public string BackgroundImageUrl { get; set; }

    /// <summary>
    /// 抽奖范围
    /// </summary>
    public DrawPrizeRange? DrawPrizeRange { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public DrawPrizeType? DrawPrizeType { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 中奖概率
    /// </summary>
    public double? Probability { get; set; }

    [JsonIgnore]
    public List<BrowsePrizeViewModel> Prizes { get; set; }

    /// <summary>
    /// 奖品数量
    /// </summary>
    public int PrizeCount => Prizes.Sum(x => x.Count);

    /// <summary>
    /// 中奖人数
    /// </summary>
    public int WinningPrizeRecordsCount => DrawPrizeRecords.Count(x => !string.IsNullOrEmpty(x.PrizeName));

    /// <summary>
    /// 抽奖人数
    /// </summary>
    public int DrawPrizeRecordsCount => DrawPrizeRecords.Count;

    /// <summary>
    /// 抽奖记录
    /// </summary>
    [JsonIgnore]
    public List<BrowseDrawPrizeRecordViewModel> DrawPrizeRecords { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; set; }
}