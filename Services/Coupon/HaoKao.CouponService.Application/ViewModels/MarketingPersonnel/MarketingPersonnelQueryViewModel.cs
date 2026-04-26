using HaoKao.CouponService.Domain.Queries;

namespace HaoKao.CouponService.Application.ViewModels.MarketingPersonnel;


[AutoMapFrom(typeof(MarketingPersonnelQuery))]
[AutoMapTo(typeof(MarketingPersonnelQuery))]
public class MarketingPersonnelQueryViewModel: QueryDtoBase<MarketingPersonnelQueryListViewModel>
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 手机号码
    /// </summary>
    public string TelPhone { get; set; }
}

[AutoMapFrom(typeof(Domain.Models.MarketingPersonnel))]
[AutoMapTo(typeof(Domain.Models.MarketingPersonnel))]
public class MarketingPersonnelQueryListViewModel : IDto
{

    public Guid Id { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string TelPhone { get; set; }

    public DateTime CreateTime { get; set; }
}