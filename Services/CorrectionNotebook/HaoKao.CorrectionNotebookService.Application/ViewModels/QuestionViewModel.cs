using HaoKao.CorrectionNotebookService.Domain.Enums;

namespace HaoKao.CorrectionNotebookService.Application.ViewModels;

/// <summary>
/// 题目列表查询条件
/// </summary>
/// <param name="SubjectId">科目Id</param>
/// <param name="MasteryDegree">掌握程度</param>
/// <param name="CreateTime">录入时间</param>
/// <param name="TagIds">筛选标签</param>
/// <param name="PageIndex"></param>
/// <param name="PageSize"></param>
public record QueryQuestionViewModel(
    Guid SubjectId,
    MasteryDegree? MasteryDegree,
    int? CreateTime,
    IReadOnlyList<Guid> TagIds,
    int PageSize,
    int PageIndex
) : IDto;

/// <summary>
/// 题目列表
/// </summary>
/// <param name="RecordCount">总记录数</param>
/// <param name="Result">结果集</param>
public record QuestionListViewModel(int RecordCount, IReadOnlyList<QuestionListItemViewModel> Result) : IDto;

/// <summary>
/// 题目列表项
/// </summary>
/// <param name="Id">Id</param>
/// <param name="ImageUrl">题目图片Url</param>
/// <param name="MasteryDegree">掌握程度</param>
/// <param name="CreateTime">创建时间</param>
public record QuestionListItemViewModel(Guid Id, Uri ImageUrl, MasteryDegree MasteryDegree, DateTime CreateTime) : IDto;

/// <summary>
/// 题目详情
/// </summary>
/// <param name="Id"></param>
/// <param name="SubjectId">科目Id</param>
/// <param name="ImageUrl">题目图片Url</param>
/// <param name="Answer">题目答案</param>
/// <param name="Analysis">题目解析</param>
/// <param name="MasteryDegree">掌握程度</param>
/// <param name="CreateTime">创建时间</param>
/// <param name="IsGenerated">是否使用过精准解析</param>
/// <param name="Tags">标签集</param>
public record QuestionViewModel(
    Guid Id,
    Guid SubjectId,
    Uri ImageUrl,
    string Answer,
    string Analysis,
    MasteryDegree MasteryDegree,
    DateTime CreateTime,
    bool IsGenerated,
    IReadOnlyList<TagCategoryItemViewModel> Tags
) : IDto;

/// <summary>
/// 创建题目
/// </summary>
/// <param name="SubjectId">科目Id</param>
/// <param name="ImageUrls">题目图片Url</param>
/// <param name="TagIds">标签Id集合</param>
public record CreateQuestionViewModel(
    Guid SubjectId,
    IReadOnlyList<Uri> ImageUrls,
    IReadOnlyList<Guid> TagIds
) : IDto;

/// <summary>
/// 生成的答案与解析
/// </summary>
/// <param name="Answer"></param>
/// <param name="Analysis"></param>
public record GetAnswerAndAnalysisViewModel(string Answer, string Analysis) : IDto;

/// <summary>
/// 修改题目掌握程度
/// </summary>
/// <param name="Ids"></param>
/// <param name="MasteryDegree"></param>
public record EditQuestionMasteryDegreeViewModel(IReadOnlyList<Guid> Ids, MasteryDegree MasteryDegree) : IDto;

/// <summary>
/// 修改题目标签
/// </summary>
/// <param name="Id"></param>
/// <param name="TagIds"></param>
public record EditQuestionTagViewModel(Guid Id, IReadOnlyList<Guid> TagIds) : IDto;