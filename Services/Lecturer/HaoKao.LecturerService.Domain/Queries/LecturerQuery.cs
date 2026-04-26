using Girvs.Extensions;
using HaoKao.LecturerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HaoKao.LecturerService.Domain.Queries;

public class LecturerQuery : QueryBase<Lecturer>
{
    /// <summary>
    /// 名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 所属科目
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 简介
    /// </summary>
    [QueryCacheKey]
    public string Desc { get; set; }

    [QueryCacheKey]
    public string CourseIntroduction { get; set; }

    public override Expression<Func<Lecturer, bool>> GetQueryWhere()
    {
        Expression<Func<Lecturer, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }
        if (SubjectId.HasValue)
        {
            expression = expression.And(x => EF.Functions.JsonContains(x.SubjectIds, $"\"{SubjectId}\""));
        }
        if (!string.IsNullOrEmpty(Desc))
        {
            expression = expression.And(x => x.Desc.Contains(Desc));
        }
        if (!string.IsNullOrEmpty(CourseIntroduction))
        {
            expression = expression.And(x => x.Desc.Contains(CourseIntroduction));
        }

        return expression;
    }
}