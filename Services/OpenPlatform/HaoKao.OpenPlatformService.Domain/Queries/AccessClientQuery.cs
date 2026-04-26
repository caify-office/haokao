namespace HaoKao.OpenPlatformService.Domain.Queries;

public class AccessClientQuery : QueryBase<AccessClient>
{
    /// <summary>
    /// 客户端标识
    /// </summary>
    [QueryCacheKey]
    public string ClientId { get; set; }

    /// <summary>
    /// 客户端名称
    /// </summary>
    [QueryCacheKey]
    public string ClientName { get; set; }

    public override Expression<Func<AccessClient, bool>> GetQueryWhere()
    {
        Expression<Func<AccessClient, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(ClientId))
        {
            expression = expression.And(x => x.ClientId.Contains(ClientId));
        }
        if (!string.IsNullOrEmpty(ClientName))
        {
            expression = expression.And(x => x.ClientName.Contains(ClientName));
        }
        return expression;
    }
}