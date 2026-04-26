using HaoKao.NoticeService.Domain.Models;

namespace HaoKao.NoticeService.Domain.Queries;

public class NoticeQuery : QueryBase<Notice>
{
    /// <summary>
    /// 公告名称
    /// </summary>
    [QueryCacheKey]
    public string Title { get; set; }

    /// <summary>
    /// 是否弹出
    /// </summary>
    [QueryCacheKey]
    public bool? Popup { get; set; }

    /// <summary>
    /// 是否发布
    /// </summary>
    [QueryCacheKey]
    public bool? Published { get; set; }

    public override Expression<Func<Notice, bool>> GetQueryWhere()
    {
        Expression<Func<Notice, bool>> expression = _ => true;

        if (!string.IsNullOrEmpty(Title))
        {
            expression = expression.And(x => EF.Functions.Like(x.Title, $"%{Title}%"));
        }
        if (Popup == true)
        {
            expression = expression.And(x => x.Popup)
                                   .And(x => x.StartTime <= DateTime.Now)
                                   .And(x => x.EndTime > DateTime.Now);
        }
        if (Published.HasValue)
        {
            expression = expression.And(x => x.Published == Published.Value);
        }

        return expression;
    }
}