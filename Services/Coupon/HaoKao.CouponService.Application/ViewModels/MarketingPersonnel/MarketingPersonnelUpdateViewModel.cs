using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.CouponService.Application.ViewModels.MarketingPersonnel;


[AutoMapTo(typeof(Domain.Commands.MarketingPersonnel.UpdateMarketingPersonnelCommand))]
public class UpdateMarketingPersonnelViewModel : IDto
{
    public Guid   Id { get; set; }
    /// <summary>
    /// 姓名
    /// </summary>
    [DisplayName("姓名")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Name{ get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [DisplayName("手机号码")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string TelPhone{ get; set; }
}