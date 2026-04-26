using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Domain.QuestionWrongModule;

namespace HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels;

[AutoMapFrom(typeof(QuestionWrongQuery))]
[AutoMapTo(typeof(QuestionWrongQuery))]
public class QueryMakingPaperViewModel : QueryDtoBase<MakingPaperViewModel>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 是否激活状态的错题, true 待消灭, false 被消灭
    /// </summary>
    public bool? IsActive { get; init; }

    /// <summary>
    /// 题型Id集合
    /// </summary>
    public IReadOnlyList<Guid> QuestionTypeIds { get; init; } = [];
}

/// <summary>
/// 错题组卷列表返回视图
/// </summary>
[AutoMapFrom(typeof(QuestionWrong))]
[AutoMapTo(typeof(QuestionWrong))]
public class MakingPaperViewModel : IDto
{
    /// <summary>
    /// 题目Id
    /// </summary>
    public Guid QuestionId { get; init; }

    /// <summary>
    /// 父Id
    /// </summary>
    public string ParentId { get; set; }

    /// <summary>
    /// 父题型Id
    /// </summary>
    public Guid ParentQuestionTypeId { get; init; }

    /// <summary>
    /// 收藏时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 收藏的试题对象
    /// </summary>
    public BrowseQuestionViewModel Question { get; init; }
}