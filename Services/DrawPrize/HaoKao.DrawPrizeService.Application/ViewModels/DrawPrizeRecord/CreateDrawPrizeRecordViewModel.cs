namespace HaoKao.DrawPrizeService.Application.ViewModels.DrawPrizeRecord;


[AutoMapTo(typeof(CreateDrawPrizeRecordCommand))]
public class CreateDrawPrizeRecordViewModel : IDto
{


    /// <summary>
    /// 所属抽奖活动Id
    /// </summary>
    [DisplayName("所属抽奖活动Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid DrawPrizeId { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [DisplayName("手机号")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Phone { get; set; }

    /// <summary>
    /// 奖品名称
    /// </summary>
    [DisplayName("奖品名称")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string PrizeName { get; set; }
}