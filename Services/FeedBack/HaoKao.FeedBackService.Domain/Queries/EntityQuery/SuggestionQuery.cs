using HaoKao.FeedBackService.Domain.Entities;

namespace HaoKao.FeedBackService.Domain.Queries.EntityQuery;

public class SuggestionQuery : QueryBase<Suggestion>
{
    /// <summary>
    /// 反馈类型
    /// </summary>
    [QueryCacheKey]
    public SuggestionType? SuggestionType { get; set; }

    /// <summary>
    /// 反馈来源
    /// </summary>
    [QueryCacheKey]
    public string SuggestionFrom { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    /// <summary>
    /// 反馈人名称
    /// </summary>
    [QueryCacheKey]
    public string CreatorName { get; set; }

    /// <summary>
    /// 开始反馈时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartCreateTime { get; set; }

    /// <summary>
    /// 结束反馈时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndCreateTime { get; set; }

    /// <summary>
    /// 处理状态
    /// </summary>
    [QueryCacheKey]
    public ReplyState? ReplyState { get; set; }

    /// <summary>
    /// 处理人名称
    /// </summary>
    [QueryCacheKey]
    public string ReplyUserName { get; set; }

    /// <summary>
    /// 开始回复时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartReplyTime { get; set; }

    /// <summary>
    /// 结束回复时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndReplyTime { get; set; }

    /// <summary>
    /// 反馈人Id
    /// </summary>
    [QueryCacheKey]
    public Guid? CreatorId { get; set; }

    public override Expression<Func<Suggestion, bool>> GetQueryWhere()
    {
        Expression<Func<Suggestion, bool>> expression = x => true;

        if (SuggestionType.HasValue)
        {
            expression = expression.And(x => x.SuggestionType == SuggestionType);
        }

        if (!string.IsNullOrEmpty(SuggestionFrom))
        {
            expression = expression.And(x => x.SuggestionFrom == SuggestionFrom);
        }

        if (!string.IsNullOrEmpty(Phone))
        {
            expression = expression.And(x => x.Phone == Phone);
        }

        if (!string.IsNullOrEmpty(CreatorName))
        {
            expression = expression.And(x => x.CreatorName == CreatorName);
        }

        if (StartCreateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime >= StartCreateTime);
        }

        if (EndCreateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime < EndCreateTime);
        }

        if (ReplyState.HasValue)
        {
            expression = expression.And(x => x.ReplyState == ReplyState);
        }

        if (!string.IsNullOrEmpty(ReplyUserName))
        {
            expression = expression.And(x => x.ReplyUserName == ReplyUserName);
        }

        if (StartReplyTime.HasValue)
        {
            expression = expression.And(x => x.ReplyTime >= StartReplyTime);
        }

        if (EndReplyTime.HasValue)
        {
            expression = expression.And(x => x.ReplyTime < EndReplyTime);
        }

        if (CreatorId.HasValue)
        {
            expression = expression.And(x => x.CreatorId == CreatorId);
        }

        return expression;
    }
}