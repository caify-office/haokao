using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Application.ViewModels.Prize;


[AutoMapFrom(typeof(Domain.Entities.Prize))]
public class BrowsePrizeViewModel : IDto
{

    public Guid Id { get; set; }

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
