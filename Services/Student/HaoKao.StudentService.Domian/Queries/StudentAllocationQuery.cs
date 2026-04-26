using HaoKao.StudentService.Domain.Entities;

namespace HaoKao.StudentService.Domain.Queries;

public class StudentAllocationQuery : QueryBase<StudentAllocation>
{
    /// <summary>
    /// 手机号/昵称
    /// </summary>
    [QueryCacheKey]
    public string SearchKey { get; init; }

    /// <summary>
    /// 分配销售
    /// </summary>
    [QueryCacheKey]
    public string AllocateToSalesperson { get; init; }

    /// <summary>
    /// 已加销售
    /// </summary>
    [QueryCacheKey]
    public string FollowedSalesperson { get; init; }

    /// <summary>
    /// 分配时间(开始)
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartAllocationTime { get; init; }

    /// <summary>
    /// 分配时间(结束)
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndAllocationTime { get; init; }

    /// <summary>
    /// 添加状态
    /// </summary>
    [QueryCacheKey]
    public bool? IsFollowed { get; init; }

    public override Expression<Func<StudentAllocation, bool>> GetQueryWhere()
    {
        Expression<Func<StudentAllocation, bool>> expression = x => true;

        if (!string.IsNullOrWhiteSpace(SearchKey))
        {
            expression = expression.And(x => x.Student.RegisterUser.Phone.Contains(SearchKey) || x.Student.RegisterUser.NickName.Contains(SearchKey));
        }

        if (!string.IsNullOrWhiteSpace(AllocateToSalesperson))
        {
            expression = expression.And(x => x.SalespersonName.Contains(AllocateToSalesperson));
        }

        if (!string.IsNullOrWhiteSpace(FollowedSalesperson))
        {
            expression = expression.And(x => x.Student.StudentFollows.Any(y => y.SalespersonName.Contains(FollowedSalesperson)));
        }

        if (StartAllocationTime.HasValue)
        {
            expression = expression.And(x => x.AllocationTime >= StartAllocationTime.Value);
        }

        if (EndAllocationTime.HasValue)
        {
            expression = expression.And(x => x.AllocationTime <= EndAllocationTime.Value);
        }

        if (IsFollowed.HasValue)
        {
            expression = expression.And(x => x.Student.StudentFollows.Any() == IsFollowed.Value);
        }

        return expression;
    }
}