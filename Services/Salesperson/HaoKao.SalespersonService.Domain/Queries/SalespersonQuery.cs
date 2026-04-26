using HaoKao.SalespersonService.Domain.Entities;

namespace HaoKao.SalespersonService.Domain.Queries;

public class SalespersonQuery : QueryBase<Salesperson>
{
    /// <summary>
    /// 真实姓名
    /// </summary>
    [QueryCacheKey]
    public string RealName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [QueryCacheKey]
    public Guid TenantId { get; set; }

    public override Expression<Func<Salesperson, bool>> GetQueryWhere()
    {
        Expression<Func<Salesperson, bool>> where = x => true;

        if (!string.IsNullOrWhiteSpace(RealName))
        {
            where = where.And(x => x.RealName.Contains(RealName));
        }

        if (!string.IsNullOrWhiteSpace(Phone))
        {
            where = where.And(x => x.Phone.Contains(Phone));
        }

        if (TenantId != Guid.Empty)
        {
            where = where.And(x => x.TenantId == TenantId);
        }

        return where;
    }
}