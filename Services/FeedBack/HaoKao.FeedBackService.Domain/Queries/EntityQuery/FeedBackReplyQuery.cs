using HaoKao.FeedBackService.Domain.Entities;

namespace HaoKao.FeedBackService.Domain.Queries.EntityQuery;

public class FeedBackReplyQuery : QueryBase<FeedBackReply>
{   
    public override Expression<Func<FeedBackReply, bool>> GetQueryWhere()
    {
        Expression<Func<FeedBackReply, bool>> expression = x => true;
        return expression;
    }
}
