using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Queries;

public class SensitiveWordQuery : QueryBase<SensitiveWord>
{
    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }

    public override Expression<Func<SensitiveWord, bool>> GetQueryWhere()
    {
        Expression<Func<SensitiveWord, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(Content))
        {
            expression = expression.And(x => x.Content.Contains(Content));
        }
        return expression;
    }
}