using HaoKao.StudentService.Domain.Entities;

namespace HaoKao.StudentService.Domain.Queries;

public class StudentParameterConfigQuery : QueryBase<StudentParameterConfig>
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [QueryCacheKey]
    public Guid? UserId { get; init; }

    /// <summary>
    /// 设置值字段名称
    /// </summary>
    [QueryCacheKey]
    public string PropertyName { get; init; }

    /// <summary>
    /// 设置值类型
    /// </summary>
    [QueryCacheKey]
    public string PropertyType { get; init; }

    /// <summary>
    /// 设置值
    /// </summary>
    [QueryCacheKey]
    public string PropertyValue { get; init; }

    public override Expression<Func<StudentParameterConfig, bool>> GetQueryWhere()
    {
        Expression<Func<StudentParameterConfig, bool>> expression = x => true;

        if (UserId.HasValue)
        {
            expression = expression.And(x => x.UserId == UserId);
        }

        if (!string.IsNullOrEmpty(PropertyName))
        {
            expression = expression.And(x => x.PropertyName == PropertyName);
        }

        if (!string.IsNullOrEmpty(PropertyType))
        {
            expression = expression.And(x => x.PropertyType == PropertyType);
        }

        if (!string.IsNullOrEmpty(PropertyValue))
        {
            expression = expression.And(x => x.PropertyValue == PropertyValue);
        }

        return expression;
    }
}
