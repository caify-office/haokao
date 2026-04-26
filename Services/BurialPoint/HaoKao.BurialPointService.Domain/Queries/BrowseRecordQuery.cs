using Girvs.Extensions;
using HaoKao.BurialPointService.Domain.Entities;

namespace HaoKao.BurialPointService.Domain.Queries;

public class BrowseRecordQuery : QueryBase<BrowseRecord>
{   /// <summary>
    /// 开始时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartTime { get; set; }


    /// <summary>
    /// 结束时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndTime { get; set; }
    /// <summary>
    /// 埋点id
    /// </summary>
    [QueryCacheKey]
    public Guid? BurialPointId { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }
    public override Expression<Func<BrowseRecord, bool>> GetQueryWhere()
    {
        Expression<Func<BrowseRecord, bool>> expression = x => true;
        if (StartTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime > StartTime);
        }
        if (EndTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime < EndTime);
        }
        if (BurialPointId.HasValue)
        {
            expression = expression.And(x => x.BurialPointId == BurialPointId);
        }
        if (!string.IsNullOrEmpty(Phone))
        {
            expression = expression.And(x => x.Phone.Contains(Phone) || x.UserName.Contains(Phone));
        }
        return expression;
    }
}
