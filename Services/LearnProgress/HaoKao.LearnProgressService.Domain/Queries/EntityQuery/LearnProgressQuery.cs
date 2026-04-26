using Girvs.Extensions;
using HaoKao.LearnProgressService.Domain.Entities;
using System;


namespace HaoKao.LearnProgressService.Domain.Queries.EntityQuery;

public class LearnProgressQuery : QueryBase<LearnProgress>
{
    /// <summary>
    /// 用户
    /// </summary>
    public Guid? CreatorId { get; set; }

    /// <summary>
    /// 需要查询的视频集合
    /// </summary>
    public string VideoIds { get; set; }
    public override Expression<Func<LearnProgress, bool>> GetQueryWhere()
    {
        Expression<Func<LearnProgress, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(VideoIds))
            expression = expression.And(x => VideoIds.Contains(x.VideoId.ToString()));
        else expression = expression.And(x => x.VideoId==Guid.Empty);
        if (CreatorId.HasValue)
        {
            expression=expression.And(x => x.CreatorId == CreatorId);


        }
        return expression;
    }
}
