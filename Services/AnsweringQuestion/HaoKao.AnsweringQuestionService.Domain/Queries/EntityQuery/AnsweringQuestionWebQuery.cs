using Girvs.Extensions;
using HaoKao.AnsweringQuestionService.Domain.Entities;
using HaoKao.AnsweringQuestionService.Domain.Enumerations;
using System;

namespace HaoKao.AnsweringQuestionService.Domain.Queries.EntityQuery;

public class AnsweringQuestionWebQuery : QueryBase<AnsweringQuestion>
{
    public Guid? SubjectId { get; set; }
    public Guid? UserId { get; set; }

    public Guid? ProductId { get; set; }

    /// <summary>
    /// 课程id
    /// </summary>
    [QueryCacheKey]
    public Guid? CourseId { get; set; }

    /// <summary>
    /// 关键词搜索
    /// </summary>
    [QueryCacheKey]
    public string SearchKey { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    [QueryCacheKey]
    public AnsweringQuestionEnum? Type { get; set; }
    public override Expression<Func<AnsweringQuestion, bool>> GetQueryWhere()
    {
        Expression<Func<AnsweringQuestion, bool>> expression = x => true;
        if (ProductId.HasValue)
        {
            expression = expression.And(x => x.ProductId == ProductId);
        }
        if (SubjectId.HasValue)
        {
            expression = expression.And(x => x.SubjectId == SubjectId);
        }
        if (CourseId.HasValue)
        {
            expression = expression.And(x => x.CourseId== CourseId);
        }
        if (UserId.HasValue)
        {
            expression = expression.And(x => x.CreatorId == UserId);
        }
        else
        {
            //热门提问只收录已回答状态的问题
            expression = expression.And(x => x.IsReply == true);
        }

        if (!string.IsNullOrEmpty(SearchKey))
        {
            expression = expression.And(x => x.Description.Contains(SearchKey));
        }
        if (Type != null)
        {
            expression = expression.And(x => x.Type == Type);
        }
        return expression;
    }
}
