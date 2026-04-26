namespace HaoKao.DrawPrizeService.Application.ViewModels.DrawPrizeRecord;


[AutoMapFrom(typeof(Domain.Entities.DrawPrizeRecord))]
public class BrowseDrawPrizeRecordViewModel : IDto
{
    public Guid  Id { get; set; }
    /// <summary>
    /// 所属抽奖活动Id
    /// </summary>
    public Guid DrawPrizeId { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 中奖奖品Id
    /// </summary>
    public Guid PrizeId { get; set; }

    /// <summary>
    /// 奖品名称
    /// </summary>
    public string PrizeName { get; set; }
}
