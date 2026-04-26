using Girvs.Extensions;
using HaoKao.QuestionCategoryService.Domain.Entities;
using HaoKao.QuestionCategoryService.Domain.Enums;
using System;

namespace HaoKao.QuestionCategoryService.Domain.Queries;

public class QuestionCategoryQuery : QueryBase<QuestionCategory>
{
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 适应场景
    /// </summary>
    [QueryCacheKey]
    public AdaptPlace? AdaptPlace { get; set; }

    public override Expression<Func<QuestionCategory, bool>> GetQueryWhere()
    {
        Expression<Func<QuestionCategory, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }

        if (AdaptPlace.HasValue)
        {
            expression = expression.And(x => x.AdaptPlace == AdaptPlace);
        }
        return expression;
    }
}