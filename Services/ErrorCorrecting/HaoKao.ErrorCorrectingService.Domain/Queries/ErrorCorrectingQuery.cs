using Girvs.Extensions;
using HaoKao.ErrorCorrectingService.Domain.Entities;
using System;

namespace HaoKao.ErrorCorrectingService.Domain.Queries;

public class ErrorCorrectingQuery : QueryBase<ErrorCorrecting>
{
    /// <summary>
    /// 科目id
    /// </summary>
    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 分类ID
    /// </summary>
    [QueryCacheKey]
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// 类别id
    /// </summary>
    [QueryCacheKey]
    public Guid? QuestionTypeId { get; set; }

    /// <summary>
    /// 题干
    /// </summary>
    [QueryCacheKey]
    public string QuestionText { get; set; }

    /// <summary>
    /// 昵称/手机号码
    /// </summary>
    [QueryCacheKey]
    public string SearchKey { get; set; }

    /// <summary>
    ///错误类型
    /// </summary>
    [QueryCacheKey]
    public string ErrorTypes { get; set; }

    public override Expression<Func<ErrorCorrecting, bool>> GetQueryWhere()
    {
        Expression<Func<ErrorCorrecting, bool>> expression = x => true;
        if (SubjectId.HasValue)
        {
            expression = expression.And(x => x.SubjectId == SubjectId);
        }
        if (CategoryId.HasValue)
        {
            expression = expression.And(x => x.CategoryId == CategoryId);
        }
        if (QuestionTypeId.HasValue)
        {
            expression = expression.And(x => x.QuestionTypeId == QuestionTypeId);
        }
        if (!string.IsNullOrEmpty(SearchKey))
        {
            expression = expression.And(x => x.NickName.Contains(SearchKey) || x.Phone.Contains(SearchKey));
        }
        if (!string.IsNullOrEmpty(QuestionText))
        {
            expression = expression.And(x => x.QuestionText.Contains(QuestionText));
        }
        if (!string.IsNullOrEmpty(ErrorTypes))
        {
            expression = expression.And(x => x.QuestionTypes.Contains(ErrorTypes));
        }
        return expression;
    }
}