namespace HaoKao.DrawPrizeService.Application.ViewModels.DrawPrize;


[AutoMapTo(typeof(CreateDrawPrizeCommand))]
public class CreateDrawPrizeViewModel : IDto
{

    /// <summary>
    /// 名称
    /// </summary>
    [DisplayName("名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Title { get; set; }

    /// <summary>
    /// 活动背景图
    /// </summary>
    [DisplayName("活动背景图")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(1000, ErrorMessage = "{0}长度不能大于{1}")]
    public string BackgroundImageUrl { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [DisplayName("开始时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [DisplayName("结束时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [DisplayName("说明")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Desc { get; set; }

}