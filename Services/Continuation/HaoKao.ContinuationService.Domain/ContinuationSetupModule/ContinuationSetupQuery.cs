namespace HaoKao.ContinuationService.Domain.ContinuationSetupModule;

public class ContinuationSetupQuery : QueryBase<ContinuationSetup>
{
    /// <summary>
    /// 是否启用
    /// </summary>
    [QueryCacheKey]
    public bool? Enable { get; set; }

    public override Expression<Func<ContinuationSetup, bool>> GetQueryWhere()
    {
        Expression<Func<ContinuationSetup, bool>> expression = x => !x.IsDelete;

        if (Enable.HasValue)
        {
            expression = expression.And(x => x.Enable == Enable);
        }

        return expression;
    }
}