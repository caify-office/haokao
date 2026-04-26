namespace HaoKao.DrawPrizeService.Application.ViewModels.DrawPrizeRecord;


[AutoMapFrom(typeof(DrawPrizeRecordQuery))]
[AutoMapTo(typeof(DrawPrizeRecordQuery))]
public class DrawPrizeRecordQueryViewModel : QueryDtoBase<DrawPrizeRecordQueryListViewModel>
{
    /// <summary>
    /// 创建者id
    /// </summary>
    public Guid? CreatorId { get; set; }
    /// <summary>
    /// 所属抽奖活动Id
    /// </summary>
    public Guid? DrawPrizeId { get; set; }
    /// <summary>
    /// 创建者名称
    /// </summary>
    public string CreatorName { get; set; }
    /// <summary>
    /// 奖品名称
    /// </summary>
    public string PrizeName { get; set; }
    /// <summary>
    /// 是否中奖（true：返回中奖记录，false：返回没中奖记录，不传：都返回）
    /// </summary>
    public bool? IsWinning { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.DrawPrizeRecord))]
[AutoMapTo(typeof(Domain.Entities.DrawPrizeRecord))]
public class DrawPrizeRecordQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 创建者名称
    /// </summary>
    public string CreatorName { get; set; }

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