using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.CouponService.Application.ViewModels.UserCouponPerformance;

[AutoMapTo(typeof(Domain.Commands.UserCouponPerformance.UpdateUserCouponPerformanceCommand))]
public class UpdateUserCouponPerformanceViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 助教实名名称
    /// </summary>
    [DisplayName("助教实名名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string PersonName { get; set; }

    /// <summary>
    /// 营销助教userid
    /// </summary>
    [DisplayName("营销助教userid")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid PersonUserId { get; set; }
  
}