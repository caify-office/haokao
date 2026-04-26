using HaoKao.QuestionService.Domain.QuestionWrongPaperMoudle;

namespace HaoKao.QuestionService.Application.QuestionWrongPaperModule;

[AutoMapTo(typeof(QuestionWrongPaperQuery))]
[AutoMapFrom(typeof(QuestionWrongPaperQuery))]
public class QueryQuestionWrongPaperViewModel : QueryDtoBase<BrowseQuestionWrongPaperViewModel>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    [Required]
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 组卷时间范围
    /// </summary>
    public int? CreateDateRange { get; init; }

    /// <summary>
    /// 创建者Id
    /// </summary>
#pragma warning disable CA1822 // 将成员标记为 static
    public Guid CreatorId => EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
#pragma warning restore CA1822 // 将成员标记为 static
}

[AutoMapTo(typeof(Domain.QuestionWrongPaperMoudle.QuestionWrongPaper))]
[AutoMapFrom(typeof(Domain.QuestionWrongPaperMoudle.QuestionWrongPaper))]
public record BrowseQuestionWrongPaperViewModel : IDto
{
    /// <summary>
    /// 试卷Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 试卷名称
    /// </summary>
    public string PaperName { get; init; }

    /// <summary>
    /// 试卷下载地址
    /// </summary>
    public Uri DownloadUrl { get; init; }

    /// <summary>
    /// 试题数量
    /// </summary>
    public int QuestionCount { get; init; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}

/// <summary>
/// 创建错题组卷视图模型
/// </summary>
/// <param name="SubjectId">科目Id</param>
/// <param name="PaperName">试卷名称</param>
/// <param name="DownloadUrl">下载链接</param>
/// <param name="QuestionCount">题目数量</param>
public record CreateQuestionWrongPaperViewModel(Guid SubjectId, string PaperName, Uri DownloadUrl, int QuestionCount) : IDto;