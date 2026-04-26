namespace HaoKao.CouponService.Application.ViewModels.MarketingPersonnel;


[AutoMapFrom(typeof(Domain.Models.MarketingPersonnel))]
public class BrowseMarketingPersonnelViewModel : IDto
{

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name{ get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string TelPhone{ get; set; }
}
