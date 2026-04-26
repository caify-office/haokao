using HaoKao.UserAnswerRecordService.Application.Helpers;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Queries;

namespace HaoKao.UserAnswerRecordService.Application.Modules.ProductRecordModule;

[AutoMapTo(typeof(ProductKnowledgeAnswerRecordQuery))]
[AutoMapFrom(typeof(ProductKnowledgeAnswerRecordQuery))]
public class QueryProductKnowledgeListViewModel : QueryDtoBase<ProductKnowledgePointRecordViewModel>
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid? SubjectId { get; init; }

    /// <summary>
    /// 章节Id
    /// </summary>
    public Guid? ChapterId { get; init; }

    /// <summary>
    /// 小节Id
    /// </summary>
    public Guid? SectionId { get; init; }

    /// <summary>
    /// 掌握程度
    /// </summary>
    public MasteryLevel? MasteryLevel { get; init; }

    /// <summary>
    /// 知识点Ids
    /// </summary>
    [JsonIgnore, Newtonsoft.Json.JsonIgnore]
    public IReadOnlyList<Guid> KnowledgePointIds { get; init; }
}

[AutoMapTo(typeof(ProductKnowledgeAnswerRecord))]
[AutoMapFrom(typeof(ProductKnowledgeAnswerRecord))]
public record ProductKnowledgePointRecordViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 知识点Id
    /// </summary>
    public Guid KnowledgePointId { get; init; }

    /// <summary>
    /// 正确率
    /// </summary>
    public int CorrectRate { get; init; }

    /// <summary>
    /// 掌握程度
    /// </summary>
    public MasteryLevel MasteryLevel { get; init; }
}

public record QueryPaperRecordIdsViewModel(Guid ProductId, IReadOnlyList<Guid> PaperIds) : IDto;

public record QueryChapterRecordIdsViewModel(Guid ProductId, IReadOnlyList<Guid> ChapterIds) : IDto;

/// <summary>
/// 题目掌握程度统计
/// </summary>
public record KnowledgeMasteryStat
{
    /// <summary>
    /// 全部知识点数量
    /// </summary>
    public int Total { get; init; }

    /// <summary>
    /// 已掌握数量
    /// </summary>
    public int Mastered { get; init; }

    /// <summary>
    /// 待加强数量
    /// </summary>
    public int NeedsImprovement { get; init; }

    /// <summary>
    /// 未掌握数量
    /// </summary>
    public int NotMastered { get; init; }
}

/// <summary>
/// 按考频统计知识点掌握情况
/// </summary>
public record ExamFrequencyMasteryViewModel
{
    /// <summary>
    /// 高频知识点统计
    /// </summary>
    public ExamFrequencyMasteryDetail High { get; set; } = new();

    /// <summary>
    /// 中频知识点统计
    /// </summary>
    public ExamFrequencyMasteryDetail Medium { get; set; } = new();

    /// <summary>
    /// 低频知识点统计
    /// </summary>
    public ExamFrequencyMasteryDetail Low { get; set; } = new();
}

/// <summary>
/// 掌握情况统计详情
/// </summary>
public record ExamFrequencyMasteryDetail
{
    /// <summary>
    /// 已掌握的知识点数量
    /// </summary>
    public int MasteredCount { get; init; }

    /// <summary>
    /// 未掌握的知识点数量 (包含待加强和未掌握)
    /// </summary>
    public int NotMasteredCount { get; init; }
}

public record QueryDateStudyDurationViewModel
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateOnly StartDate { get; init; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateOnly EndDate { get; init; }
}

public record DateStudyDurationViewModel
{
    /// <summary>
    /// 日期
    /// </summary>
    public DateOnly Date { get; init; }

    /// <summary>
    /// 时长 (小时)
    /// </summary>
    public double Duration { get; init; }
}

public record SubjectAnswerStatViewModel
{
    /// <summary>
    /// 总题数
    /// </summary>
    public int QuestionCount { get; init; }

    /// <summary>
    /// 作答数
    /// </summary>
    public int AnswerCount { get; init; }

    /// <summary>
    /// 正确数
    /// </summary>
    public int CorrectCount { get; init; }

    /// <summary>
    /// 正确率
    /// </summary>
    public int CorrectRate => PercentageHelper.CalculatePercentage(CorrectCount, QuestionCount);
}