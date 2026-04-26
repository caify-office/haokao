using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Application.ViewModels.Prize;


[AutoMapFrom(typeof(PrizeQuery))]
[AutoMapTo(typeof(PrizeQuery))]
public class PrizeQueryViewModel : QueryDtoBase<PrizeQueryListViewModel>
{
    /// <summary>
    /// 所属抽奖活动Id
    /// </summary>
    public Guid? DrawPrizeId { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.Prize))]
[AutoMapTo(typeof(Domain.Entities.Prize))]
public class PrizeQueryListViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 所属抽奖活动Id
    /// </summary>
    public Guid DrawPrizeId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 奖品数量
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 显示数量
    /// </summary>
    public int DisplayCount { get; set; }

    /// <summary>
    /// 中奖范围
    /// </summary>
    public WinningRange WinningRange { get; set; }

    /// <summary>
    ///  指定学员
    /// </summary>
    public List<DesignatedStudent> DesignatedStudents { get; set; }

    /// <summary>
    /// 是否保底
    /// </summary>
    public bool IsGuaranteed { get; set; }

    /// <summary>
    /// 已颁发的奖品数量
    /// </summary>
    public int AwardedQuantity { get; set; }

}