using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Application.Helpers;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Application.Modules.AnswerRecordModule;

public interface IPaperAnswerRecordService : IManager
{
    Task<PaperAnswerRecordViewModel> Get(Guid id);

    Task<IReadOnlyList<PaperRecordViewModel>> GetPaperList(Guid categoryId, Guid subjectId);

    Task<IReadOnlyList<PaperRecordViewModel>> GetPaperList(IReadOnlyList<Guid> paperIds);
}

/// <summary>
/// 试卷答题记录服务
/// </summary>
/// <param name="mapper"></param>
/// <param name="cacheManager"></param>
/// <param name="repository"></param>
public class PaperAnswerRecordService(
    IMapper mapper,
    IStaticCacheManager cacheManager,
    IPaperAnswerRecordRepository repository
) : IPaperAnswerRecordService
{
    private readonly Guid _userId = EngineContext.Current.IsAuthenticated
        ? EngineContext.Current.ClaimManager.GetUserId().To<Guid>()
        : Guid.Empty;

    /// <summary>
    /// 按Id获取记录(答题结果页面)
    /// </summary>
    /// <param name="id">试卷Id</param>
    /// <returns></returns>
    public async Task<PaperAnswerRecordViewModel> Get(Guid id)
    {
        var cacheKey = GirvsEntityCacheDefaults<PaperAnswerRecord>.ByIdCacheKey.Create(id.ToString());
        var entity = await cacheManager.GetAsync(cacheKey, () => repository.GetById(id));
        // var entity = await repository.GetById(id);
        return mapper.Map<PaperAnswerRecordViewModel>(entity);
    }

    /// <summary>
    /// 获取试卷列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    public async Task<IReadOnlyList<PaperRecordViewModel>> GetPaperList(Guid categoryId, Guid subjectId)
    {
        var key = $"userId_{_userId}:subjectId_{subjectId}:categoryId_{categoryId}";
        var cacheKey = GirvsEntityCacheDefaults<PaperAnswerRecord>.QueryCacheKey.Create(key);
        var list = await cacheManager.GetAsync(cacheKey, () => repository.GetPaperList(_userId, subjectId, categoryId));
        // var list = await repository.GetPaperList(_userId, subjectId, categoryId);
        return mapper.Map<IReadOnlyList<PaperRecordViewModel>>(list);
    }

    /// <summary>
    /// 获取试卷列表
    /// </summary>
    /// <param name="paperIds">试卷Ids</param>
    /// <returns></returns>
    public async Task<IReadOnlyList<PaperRecordViewModel>> GetPaperList(IReadOnlyList<Guid> paperIds)
    {
        var list = await repository.GetPaperList(_userId, paperIds);
        var dict = await repository.GetPaperUserCount(paperIds);

        var result = from id in paperIds
                     let item = list.FirstOrDefault(x => x.PaperId == id)
                     where item != null
                     select new PaperRecordViewModel
                     {
                         SubjectId = item.SubjectId,
                         CategoryId = item.CategoryId,
                         PaperId = item.PaperId,
                         UserScore = item.UserScore,
                         PassingScore = item.PassingScore,
                         TotalScore = item.TotalScore,
                         CorrectRate = PercentageHelper.CalculatePercentage(item.AnswerRecord.CorrectCount, item.AnswerRecord.QuestionCount),
                         Progress = PercentageHelper.CalculatePercentage(item.AnswerRecord.AnswerCount, item.AnswerRecord.QuestionCount),
                         CreateTime = item.CreateTime,
                         CompletedUserCount = dict.TryGetValue(id, out var val) ? val : 0
                     };

        return result.ToList();
    }
}

[AutoMapFrom(typeof(PaperAnswerRecord))]
public record PaperRecordViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 分类Id
    /// </summary>
    public Guid CategoryId { get; init; }

    /// <summary>
    /// 试卷Id
    /// </summary>
    public Guid PaperId { get; init; }

    /// <summary>
    /// 用户得分
    /// </summary>
    public decimal UserScore { get; init; }

    /// <summary>
    /// 及格分数
    /// </summary>
    public decimal PassingScore { get; init; }

    /// <summary>
    /// 试题总分
    /// </summary>
    public decimal TotalScore { get; init; }

    /// <summary>
    /// 正确率
    /// </summary>
    public int CorrectRate { get; init; }

    /// <summary>
    /// 进度
    /// </summary>
    public int Progress { get; init; }

    /// <summary>
    /// 答题时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 已完成人数
    /// </summary>
    public int CompletedUserCount { get; set; }
}

[AutoMapFrom(typeof(PaperAnswerRecord))]
public record PaperAnswerRecordViewModel : PaperRecordViewModel
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
    /// 少选数
    /// </summary>
    public int LackCount => Questions.Count(x => x.JudgeResult == ScoringRuleType.Lack);

    /// <summary>
    /// 总耗时
    /// </summary>
    public long ElapsedTime { get; init; }

    /// <summary>
    /// 作答试题
    /// </summary>
    public IReadOnlyList<AnswerQuestionViewModel> Questions { get; init; } = [];
}