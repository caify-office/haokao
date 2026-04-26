using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Repositories;

namespace HaoKao.OrderService.Infrastructure.Repositories;

public class OrderInvoiceRepository : Repository<OrderInvoice>, IOrderInvoiceRepository
{
    /// <summary>
    /// 查询详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public override Task<OrderInvoice> GetByIdAsync(Guid id)
    {
        return Queryable
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<List<OrderInvoice>> GetByQueryAsync(QueryBase<OrderInvoice> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await Queryable
                                 .Include(x => x.Order)
                                 .Where(query.GetQueryWhere())
                                 .SelectProperties(query.QueryFields)
                                 .OrderByDescending(x => x.CreateTime)
                                 .Skip(query.PageStart)
                                 .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}