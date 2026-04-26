namespace HaoKao.DrawPrizeService.Application.ViewModels.Prize;


[AutoMapTo(typeof(SetPrizeGuaranteedCommand))]
public class SetPrizeGuaranteedPrizeViewModel : IDto
{

    public Guid Id { get; set; }
    /// <summary>
    /// 所属抽奖活动Id
    /// </summary>
    [DisplayName("所属抽奖活动Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid DrawPrizeId { get; set; }

}