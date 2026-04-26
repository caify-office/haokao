using HaoKao.OrderService.Domain.Entities;

namespace HaoKao.OrderService.Domain.Queries;

public class ProducSalesQuery : QueryBase<ProductSales>
{
    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public bool? IsExport { get; set; }

    public override Expression<Func<ProductSales, bool>> GetQueryWhere()
    {
        Expression<Func<ProductSales, bool>> where = x => true;

        if (StartTime.HasValue)
        {
            where = where.And(x => x.CreateTime >= StartTime.Value);
        }

        if (EndTime.HasValue)
        {
            where = where.And(x => x.CreateTime <= EndTime.Value);
        }

        return where;
    }
}