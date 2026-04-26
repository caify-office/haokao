using Girvs.BusinessBasis.Queries;
using HaoKao.ErrorCorrectingService.Domain.Entities;
using HaoKao.ErrorCorrectingService.Domain.Queries;

namespace HaoKao.ErrorCorrectingService.Application.ViewModels.ErrorCorrecting;

[AutoMapFrom(typeof(ErrorCorrectingQuery))]
[AutoMapTo(typeof(ErrorCorrectingQuery))]
public class ErrorCorrectingQueryViewModel : QueryDtoBase<ErrorCorrectingQueryListViewModel>
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
}

[AutoMapFrom(typeof(Domain.Entities.ErrorCorrecting))]
[AutoMapTo(typeof(Domain.Entities.ErrorCorrecting))]
public class ErrorCorrectingQueryListViewModel : IDto
{
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 问题id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 用户id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 问题类型
    /// </summary>
    public string QuestionTypes { get; set; }

    /// <summary>
    /// 时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    public Guid SubjectId { get; set; } //科目ID

    public string SubjectName { get; set; } //科目名称

    public Guid QuestionTypeId { get; set; } //题型类别id

    public string QuestionTypeName { get; set; } //题型类别名称

    public string QuestionText { get; set; } //题干

    public string NickName { get; set; } //昵称

    public string Phone { get; set; } //手机号码

    public Guid CategoryId { get; set; } //分类id

    public string CategoryName { get; set; } //分类名称

    public StatusEnum Status { get; set; } //错题处理状态
}