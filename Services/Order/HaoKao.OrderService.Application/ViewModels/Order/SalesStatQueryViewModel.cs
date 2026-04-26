using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.ValueObjects;

namespace HaoKao.OrderService.Application.ViewModels.Order;

public class SalesStatQueryViewModel : QueryDtoBase<SalesStatListViewModel>
{
    /// <summary>
    /// 统计维度
    /// </summary>
    [Required]
    public SalesStatDimension Dimension { get; set; }

    /// <summary>
    /// 开始统计时间
    /// </summary>
    public string StartTime { get; set; }

    /// <summary>
    /// 开始统计时间
    /// </summary>
    internal DateOnly? StartDate
    {
        get
        {
            if (!string.IsNullOrEmpty(StartTime))
            {
                var dateTime = DateTime.TryParse(StartTime, out var value) ? value : DateTime.ParseExact(StartTime, "yyyy", CultureInfo.CurrentCulture);
                // convert to DateOnly
                return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
            }
            return null;
        }
    }

    /// <summary>
    /// 结束统计时间
    /// </summary>
    public string EndTime { get; set; }

    internal DateOnly? EndDate
    {
        get
        {
            if (!string.IsNullOrEmpty(EndTime))
            {
                var dateTime = DateTime.TryParse(EndTime, out var value) ? value : DateTime.ParseExact(EndTime, "yyyy", CultureInfo.CurrentCulture);
                // convert to DateOnly
                return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
            }
            return null;
        }
    }

    /// <summary>
    /// 租户Ids
    /// </summary>
    public List<Guid> TenantIds { get; set; }
}

[AutoMapFrom(typeof(SalesStatisticsItem))]
public class SalesStatListViewModel : IDto
{
    /// <summary>
    /// 日期
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// 订单数
    /// </summary>
    public int SumOrderCount { get; set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal SumOrderAmount { get; set; }
}

public class SalesStatDetailQueryViewModel : QueryDtoBase<SalesStatDetailListViewModel>
{
    /// <summary>
    /// 统计维度
    /// </summary>
    [Required]
    public SalesStatDimension Dimension { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    [Required]
    public string Date { get; set; }

    internal DateOnly StartDate
    {
        get
        {
            var dateTime = DateTime.TryParse(Date, out var value) ? value : DateTime.ParseExact(Date, "yyyy", CultureInfo.CurrentCulture);
            // convert to DateOnly
            return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
        }
    }

    internal DateOnly EndDate
    {
        get
        {
            {
                return Dimension switch
                {
                    SalesStatDimension.Yearly => StartDate.AddYears(1),
                    SalesStatDimension.Monthly => StartDate.AddMonths(1),
                    SalesStatDimension.Daily => StartDate.AddDays(1),
                    _ => throw new ArgumentNullException(nameof(Date))
                };
            }
        }
    }

    /// <summary>
    /// 租户Ids
    /// </summary>
    public List<Guid> TenantIds { get; set; }
}

[AutoMapFrom(typeof(SalesStatisticsItemDetail))]
public class SalesStatDetailListViewModel : IDto
{
    /// <summary>
    /// 使用的平台配置的支付者的名称
    /// </summary>
    public string PlatformPayerName { get; set; }

    /// <summary>
    /// 订单流水号
    /// </summary>
    public string OrderSerialNumber { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// 购买产品名称
    /// </summary>
    public string PurchaseName { get; set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal OrderAmount { get; set; }

    /// <summary>
    /// 实际金额
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 下单用户ID
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 下单用户名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 订单创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 订单更新时间，完成支付时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}