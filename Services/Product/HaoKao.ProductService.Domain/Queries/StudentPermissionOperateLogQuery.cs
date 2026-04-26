using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Domain.Queries;

public class StudentPermissionOperateLogQuery : QueryBase<StudentPermissionOperateLog>
{
    /// <summary>
    /// 昵称/手机号
    /// </summary>
    [QueryCacheKey]
    public string StudentName { get; set; }

    /// <summary>
    /// 开始操作时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartOperateTime { get; set; }

    /// <summary>
    /// 结束操作时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndOperateTime { get; set; }

    public override Expression<Func<StudentPermissionOperateLog, bool>> GetQueryWhere()
    {
        Expression<Func<StudentPermissionOperateLog, bool>> expression = x => true;

        if (!string.IsNullOrWhiteSpace(StudentName))
        {
            expression = expression.And(x => x.StudentName.Contains(StudentName));
        }

        if (StartOperateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime >= StartOperateTime.Value);
        }

        if (EndOperateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime <= EndOperateTime.Value);
        }

        return expression;
    }
}