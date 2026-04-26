using HaoKao.AnsweringQuestionService.Domain.Entities;
using System;

namespace HaoKao.AnsweringQuestionService.Domain.Queries.EntityQuery;

public class AnsweringQuestionReplyQuery : QueryBase<AnsweringQuestionReply>
{



    public override Expression<Func<AnsweringQuestionReply, bool>> GetQueryWhere()
    {
        Expression<Func<AnsweringQuestionReply, bool>> expression = x => true;


        return expression;
    }
}
