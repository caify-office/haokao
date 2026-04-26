using HaoKao.FeedBackService.Domain.Entities;
using HaoKao.FeedBackService.Domain.Enums;

namespace HaoKao.FeedBackService.Domain.Queries.EntityQuery;

public class FeedBackQuery : QueryBase<FeedBack>
{
    /// <summary>
    /// 反馈类型
    /// </summary>
    [QueryCacheKey]
    public TypeEnum? Type { get; set; }
    /// <summary>
    /// 来源
    /// </summary>
    [QueryCacheKey]
    public SourceTypeEnum? SourceType { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    [QueryCacheKey]
    public StatusEnum? Status { get; set; }

    /// <summary>
    /// 反馈开始时间
    /// </summary>
    [QueryCacheKey]
    public string  StartTime { get; set; }
    /// <summary>
    /// 反馈结束时间
    /// </summary>
    [QueryCacheKey]
    public string EndTime { get; set; }
    /// <summary>
    /// 处理人
    /// </summary>
    [QueryCacheKey]
    public string HanldUserName { get; set; }
    /// <summary>
    /// 处理人
    /// </summary>
    [QueryCacheKey]
    public string FeedBackUserName { get; set; }
    /// <summary>
    /// 手机号码
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    public override Expression<Func<FeedBack, bool>> GetQueryWhere()
    {
        Expression<Func<FeedBack, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(StartTime)) {
            expression = expression.And(x => x.CreateTime >=DateTime.Parse( StartTime));
        }
        if (!string.IsNullOrEmpty(EndTime))
        {
            expression = expression.And(x => x.CreateTime <= DateTime.Parse(EndTime));
        }
        if (Type!=null)
        {
            expression = expression.And(x => x.Type == Type);
        }
        if (SourceType!=null)
        {
            expression = expression.And(x => x.SourceType == SourceType);
        }
        if (Status!=null)
        {
            expression = expression.And(x => x.Status == Status);
        }
        if (!string.IsNullOrEmpty(Phone))
        {
            expression = expression.And(x => x.Contract.Contains(Phone));
        }
        if (!string.IsNullOrEmpty(FeedBackUserName))
        {
            expression = expression.And(x => x.CreatorName.Contains(FeedBackUserName));
        }
        if (Status == StatusEnum.Finished)
        {
            //只要在已完结的情况才进入处理人查询
            if (!string.IsNullOrEmpty(HanldUserName))
            {
                expression = expression.And(x => x.FeedBackReplies.Where(x=>x.CreatorName.Contains(HanldUserName)).Count()>0);
            }
           
        }
        return expression;
    }
}
