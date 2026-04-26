using HaoKao.PaperService.Domain.Entities;
using HaoKao.PaperService.Domain.Enumerations;

namespace HaoKao.PaperService.Domain.Queries;

public class PaperQuery : QueryBase<Paper>
{
    /// <summary>
    /// 试卷名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 科目id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 类别id
    /// </summary>
    [QueryCacheKey]
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// 发布状态 1--未发布 2--已发布
    /// </summary>
    [QueryCacheKey]
    public StateEnum State { get; set; }

    /// <summary>
    /// 是否限免 1-不限免 2-限免
    /// </summary>
    [QueryCacheKey]
    public FreeEnum IsFree { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    [QueryCacheKey]
    public int? Year { get; set; }

    public override Expression<Func<Paper, bool>> GetQueryWhere()
    {
        Expression<Func<Paper, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }
        if (SubjectId.HasValue)
        {
            expression = expression.And(x => x.SubjectId.Equals(SubjectId));
        }
        if (CategoryId.HasValue)
        {
            expression = expression.And(x => x.CategoryId.Equals(CategoryId));
        }
        if (State != 0)
        {
            expression = expression.And(x => x.State.Equals(State));
        }
        if (IsFree != 0)
        {
            expression = expression.And(x => x.IsFree.Equals(IsFree));
        }
        if (Year.HasValue)
        {
            expression = expression.And(x => x.Year.Equals(Year));
        }

        return expression;
    }
}