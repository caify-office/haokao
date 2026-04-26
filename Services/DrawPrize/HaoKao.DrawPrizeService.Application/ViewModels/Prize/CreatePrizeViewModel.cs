using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Application.ViewModels.Prize;


[AutoMapTo(typeof(CreatePrizeCommand))]
public class CreatePrizeViewModel : IDto
{

    /// <summary>
    /// 所属抽奖活动Id
    /// </summary>
    [DisplayName("所属抽奖活动Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid DrawPrizeId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [DisplayName("名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 奖品数量
    /// </summary>
    [DisplayName("奖品数量")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Count { get; set; }

    /// <summary>
    /// 显示数量
    /// </summary>
    [DisplayName("显示数量")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int DisplayCount { get; set; }

    /// <summary>
    /// 中奖范围
    /// </summary>
    [DisplayName("中奖范围")]
    [Required(ErrorMessage = "{0}不能为空")]
    public WinningRange WinningRange { get; set; }

    /// <summary>
    ///  指定学员
    /// </summary>
    [DisplayName("指定学员")]
    public List<DesignatedStudent> DesignatedStudents { get; set; }

    /// <summary>
    /// 是否保底
    /// </summary>
    [DisplayName("是否保底")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsGuaranteed { get; set; }

}