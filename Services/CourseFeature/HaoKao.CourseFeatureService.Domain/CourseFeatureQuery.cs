namespace HaoKao.CourseFeatureService.Domain;

public class CourseFeatureQuery : QueryBase<CourseFeature>
{
    /// <summary>
    /// 服务名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [QueryCacheKey]
    public bool? Enable { get; set; }

    public override Expression<Func<CourseFeature, bool>> GetQueryWhere()
    {
        Expression<Func<CourseFeature, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => EF.Functions.Like(x.Name, $"%{Name}%"));
        }
        if (Enable.HasValue)
        {
            expression = expression.And(x => x.Enable == Enable);
        }

        return expression;
    }
}