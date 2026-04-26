using Girvs.Extensions;
using HaoKao.AnsweringQuestionService.Domain.Entities;
using HaoKao.AnsweringQuestionService.Domain.Enumerations;
using System;

namespace HaoKao.AnsweringQuestionService.Domain.Queries.EntityQuery;

public class AnsweringQuestionQuery : QueryBase<AnsweringQuestion>
{


    /// <summary>
    /// 科目
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 提交人
    /// </summary>
    [QueryCacheKey]
    public string Submitor { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    /// <summary>
    /// 是否回复
    /// </summary>
    [QueryCacheKey]
    public bool? IsReply { get; set; }

    /// <summary>
    /// 是否新回复
    /// </summary>
    [QueryCacheKey]
    public bool? IsNewReply { get; set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    [QueryCacheKey]
    public string StartTime { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    [QueryCacheKey]
    public string EndTime { get; set; }
    /// <summary>
    /// 分类
    /// </summary>
    [QueryCacheKey]
    public AnsweringQuestionEnum? Type { get; set; }
    public override Expression<Func<AnsweringQuestion, bool>> GetQueryWhere()
    {
        Expression<Func<AnsweringQuestion, bool>> expression = x => true;
        if (SubjectId.HasValue)
        {
            expression = expression.And(x => x.SubjectId == SubjectId);
        }
        if (!string.IsNullOrEmpty(Submitor))
        {
            expression = expression.And(x => x.UserName.Contains(Submitor));
        }
        if (!string.IsNullOrEmpty(Phone))
        {
            expression = expression.And(x => x.Phone.Contains(Phone));
        }
        if (Type != null)
        {
            expression = expression.And(x => x.Type == Type);
        }
        if (IsReply != null)
        {
            expression = expression.And(x => x.IsReply == IsReply);
        }
        if (IsNewReply != null)
        {
            //存在新回复
            if (IsNewReply == true)
                expression = expression.And(x => x.AnsweringQuestionReplys.Count > 0);
            else
                expression = expression.And(x => x.AnsweringQuestionReplys.Count == 0);
        }
        if (!string.IsNullOrEmpty(StartTime))
        {
            expression = expression.And(x => x.CreateTime >= DateTime.Parse(StartTime));
        }
        if (!string.IsNullOrEmpty(EndTime))
        {
            expression = expression.And(x => x.CreateTime <= DateTime.Parse(EndTime));
        }
        return expression;
    }
}
