using HaoKao.StudentService.Domain.Entities;

namespace HaoKao.StudentService.Domain.Queries;

public class StudentQuery : QueryBase<Student>
{
    /// <summary>
    /// 昵称/手机号码
    /// </summary>
    [QueryCacheKey]
    public string SearchKey { get; init; }

    /// <summary>
    /// 是否付费学员
    /// </summary>
    [QueryCacheKey]
    public bool? IsPaidStudent { get; init; }

    /// <summary>
    /// 是否加销售
    /// </summary>
    [QueryCacheKey]
    public bool? IsFollowedSalesperson { get; init; }

    /// <summary>
    /// 销售名称
    /// </summary>
    [QueryCacheKey]
    public string SalespersonName { get; init; }

    /// <summary>
    /// 注册开始时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartTime { get; init; }

    /// <summary>
    /// 注册结束时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndTime { get; init; }

    public override Expression<Func<Student, bool>> GetQueryWhere()
    {
        Expression<Func<Student, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(SearchKey))
        {
            expression = expression.And(x => x.RegisterUser.NickName.Contains(SearchKey) || x.RegisterUser.Phone.Contains(SearchKey));
        }

        if (IsPaidStudent.HasValue)
        {
            expression = expression.And(x => x.IsPaidStudent == IsPaidStudent.Value);
        }

        if (IsFollowedSalesperson == true)
        {
            expression = expression.And(x => x.StudentFollows.Any());
        }

        if (IsFollowedSalesperson == false)
        {
            expression = expression.And(x => !x.StudentFollows.Any());
        }

        if (!string.IsNullOrEmpty(SalespersonName))
        {
            expression = expression.And(x => x.StudentFollows.Any(y => y.SalespersonName.Contains(SalespersonName)));
        }

        if (StartTime.HasValue)
        {
            expression = expression.And(x => x.RegisterUser.CreateTime >= StartTime.Value);
        }

        if (EndTime.HasValue)
        {
            expression = expression.And(x => x.RegisterUser.CreateTime < EndTime.Value);
        }

        return expression;
    }
}