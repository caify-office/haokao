using Girvs.Extensions;
using System;

namespace HaoKao.SubjectService.Domain.SubjectModule;

public class SubjectQuery : QueryBase<Subject>
{
    /// <summary>
    /// 课程名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 是否专业课程
    /// </summary>
    [QueryCacheKey]
    public SubjectTypeEnum? IsCommon { get; set; }

    public override Expression<Func<Subject, bool>> GetQueryWhere()
    {
        Expression<Func<Subject, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }

        if (IsCommon.HasValue)
        {
            expression = expression.And(x => x.IsCommon == IsCommon);
        }

        return expression;
    }
}