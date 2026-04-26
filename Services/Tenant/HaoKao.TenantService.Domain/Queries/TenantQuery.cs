using HaoKao.TenantService.Domain.Entities;
using HaoKao.TenantService.Domain.Enums;

namespace HaoKao.TenantService.Domain.Queries;

public class TenantQuery : QueryBase<Tenant>
{
    public TenantQuery()
    {
        OrderBy = nameof(Tenant.CreateTime);
    }

    /// <summary>
    /// 租户名称
    /// </summary>
    [QueryCacheKey]
    public string TenantName { get; set; }

    /// <summary>
    /// 租户代码
    /// </summary>
    [QueryCacheKey]
    public string TenantNo { get; set; }

    /// <summary>
    /// 租户启用状态
    /// </summary>
    [QueryCacheKey]
    public EnableState? StartState { get; set; }

    /// <summary>
    /// 其它名称ID，一般主要是数据字典中的Id
    /// </summary>
    [QueryCacheKey]
    public Guid? OtherId { get; set; }

    public override Expression<Func<Tenant, bool>> GetQueryWhere()
    {
        Expression<Func<Tenant, bool>> expression = exam => true;

        if (TenantName != null)
        {
            expression = expression.And(exam => exam.TenantName.Contains(TenantName));
        }

        if (TenantNo != null)
        {
            expression = expression.And(exam => exam.TenantNo == TenantNo);
        }

        if (StartState.HasValue)
        {
            expression = expression.And(exam => exam.StartState == StartState);
        }

        if (OtherId.HasValue)
        {
            expression = expression.And(exam => exam.OtherId == OtherId);
        }

        return expression;
    }
}