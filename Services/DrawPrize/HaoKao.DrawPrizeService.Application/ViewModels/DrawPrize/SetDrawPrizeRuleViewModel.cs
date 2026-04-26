namespace HaoKao.DrawPrizeService.Application.ViewModels.DrawPrize;


[AutoMapTo(typeof(SetDrawPrizeRuleCommand))]
public class SetDrawPrizeRuleViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 抽奖范围
    /// </summary>
    [DisplayName("抽奖范围")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DrawPrizeRange DrawPrizeRange { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [DisplayName("类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DrawPrizeType DrawPrizeType { get; set; }


    /// <summary>
    /// 中奖概率
    /// </summary>
    [DisplayName("中奖概率")]
    [Required(ErrorMessage = "{0}不能为空")]
    public double Probability { get; set; }

}