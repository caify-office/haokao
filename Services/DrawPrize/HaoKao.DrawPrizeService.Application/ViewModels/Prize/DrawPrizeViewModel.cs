namespace HaoKao.DrawPrizeService.Application.ViewModels.Prize;


[AutoMapTo(typeof(DrawPrizeCommand))]
public class DrawPrizeViewModel : IDto
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
    /// 是否付费学员
    /// </summary>
    [DisplayName("是否付费学员")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsPaidStudents { get; set; }

    /// <summary>
    /// 数据密钥
    /// </summary>
    [DisplayName("数据密钥")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string DataKey  { get; set; }
}