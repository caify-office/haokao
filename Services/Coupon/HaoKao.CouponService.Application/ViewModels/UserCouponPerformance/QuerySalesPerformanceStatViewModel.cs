using HaoKao.CouponService.Domain.Queries;
using HaoKao.CouponService.Domain.ValueObjects;

namespace HaoKao.CouponService.Application.ViewModels.UserCouponPerformance;

[AutoMapTo(typeof(UserCouponPerformanceQuery))]
public class QuerySalesPerformanceStatViewModel : QueryDtoBase<SalesPerformanceStatModel>
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public string StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public string EndTime { get; set; }

    /// <summary>
    /// 所选租户Id
    /// </summary>
    public List<Guid> TenantIds { get; set; }
}

[AutoMapFrom(typeof(SellerSalesStatistics))]
public class SalesPerformanceStatModel : IDto
{
    /// <summary>
    /// 销售名称
    /// </summary>
    public string PersonName { get; set; }

    /// <summary>
    /// 成交笔数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 成交总金额
    /// </summary>
    public decimal TotalAmount { get; set; }
}