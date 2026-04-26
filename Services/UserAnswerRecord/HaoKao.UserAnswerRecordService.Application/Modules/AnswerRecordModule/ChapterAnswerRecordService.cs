using HaoKao.UserAnswerRecordService.Application.Helpers;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Application.Modules.AnswerRecordModule;

public interface IChapterAnswerRecordService : IManager
{
    Task<AnswerRecordViewModel> Get(Guid id);

    Task<AnswerRecordViewModel> GetChapterAnswerRecord(Guid categoryId, Guid chapterId);

    Task<AnswerRecordViewModel> GetSectionAnswerRecord(Guid categoryId, Guid sectionId);

    Task<AnswerRecordViewModel> GetKnowledgePointAnswerRecord(Guid categoryId, Guid knowledgePointId);

    Task<IReadOnlyList<ChapterRecordViewModel>> GetChapterList(Guid categoryId, Guid subjectId);

    Task<IReadOnlyList<SectionRecordViewModel>> GetSectionList(Guid categoryId, Guid chapterId);

    Task<IReadOnlyList<KnowledgePointRecordViewModel>> GetKnowledgePointList(Guid categoryId, Guid sectionId);

    Task<ChapterRecordStat> GetChapterRecordStat(Guid subjectId);
}

/// <summary>
/// 章节答题记录服务
/// </summary>
/// <param name="mapper"></param>
/// <param name="cacheManager"></param>
/// <param name="repository"></param>
public class ChapterAnswerRecordService(
    IMapper mapper,
    IStaticCacheManager cacheManager,
    IChapterAnswerRecordRepository repository
) : IChapterAnswerRecordService
{
    private readonly Guid _userId = EngineContext.Current.IsAuthenticated
        ? EngineContext.Current.ClaimManager.GetUserId().To<Guid>()
        : Guid.Empty;

    /// <summary>
    /// 按Id获取记录(答题结果页面)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<AnswerRecordViewModel> Get(Guid id)
    {
        var cacheKey = GirvsEntityCacheDefaults<ChapterAnswerRecord>.ByIdCacheKey.Create(id.ToString());
        var entity = await cacheManager.GetAsync(cacheKey, () => repository.GetById(id));
        return mapper.Map<AnswerRecordViewModel>(entity.AnswerRecord);
    }

    /// <summary>
    /// 获取章节记录
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="chapterId">章节Id</param>
    /// <returns></returns>
    public async Task<AnswerRecordViewModel> GetChapterAnswerRecord(Guid categoryId, Guid chapterId)
    {
        var key = $"userId_{_userId}:categoryId_{categoryId}:chapterId_{chapterId}";
        var cacheKey = GirvsEntityCacheDefaults<ChapterAnswerRecord>.ByIdCacheKey.Create(key);
        var record = await cacheManager.GetAsync(cacheKey, () => repository.GetByChapterId(_userId, categoryId, chapterId));
        // var record = await repository.GetByChapterId(_userId, categoryId, chapterId);
        return mapper.Map<AnswerRecordViewModel>(record);
    }

    /// <summary>
    /// 获取小节记录
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="sectionId">小节Id</param>
    /// <returns></returns>
    public async Task<AnswerRecordViewModel> GetSectionAnswerRecord(Guid categoryId, Guid sectionId)
    {
        var key = $"userId_{_userId}:categoryId_{categoryId}:sectionId_{sectionId}";
        var cacheKey = GirvsEntityCacheDefaults<ChapterAnswerRecord>.ByIdCacheKey.Create(key);
        var record = await cacheManager.GetAsync(cacheKey, () => repository.GetBySectionId(_userId, categoryId, sectionId));
        // var record = await repository.GetBySectionId(_userId, categoryId, sectionId);
        return mapper.Map<AnswerRecordViewModel>(record);
    }

    /// <summary>
    /// 获取知识点记录
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="knowledgePointId">知识点Id</param>
    /// <returns></returns>
    public async Task<AnswerRecordViewModel> GetKnowledgePointAnswerRecord(Guid categoryId, Guid knowledgePointId)
    {
        var key = $"userId_{_userId}:categoryId_{categoryId}:knowledgePointId_{knowledgePointId}";
        var cacheKey = GirvsEntityCacheDefaults<ChapterAnswerRecord>.ByIdCacheKey.Create(key);
        var record = await cacheManager.GetAsync(cacheKey, () => repository.GetByKnowledgePointId(_userId, categoryId, knowledgePointId));
        return mapper.Map<AnswerRecordViewModel>(record);
    }

    /// <summary>
    /// 获取章节列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    public async Task<IReadOnlyList<ChapterRecordViewModel>> GetChapterList(Guid categoryId, Guid subjectId)
    {
        var key = $"userId_{_userId}:categoryId_{categoryId}:subjectId_{subjectId}";
        var cacheKey = GirvsEntityCacheDefaults<ChapterAnswerRecord>.QueryCacheKey.Create(key);
        var list = await cacheManager.GetAsync(cacheKey, () => repository.GetChapterList(_userId, categoryId, subjectId));
        // var list = await repository.GetChapterList(_userId, categoryId, subjectId);
        return mapper.Map<IReadOnlyList<ChapterRecordViewModel>>(list)
                     .GroupBy(x => x.ChapterId)
                     .Select(g => new ChapterRecordViewModel
                     {
                         ChapterId = g.Key,
                         QuestionCount = g.Sum(x => x.QuestionCount),
                         AnswerCount = g.Sum(x => x.AnswerCount),
                         CorrectCount = g.Sum(x => x.CorrectCount),
                         CreateTime = g.Max(x => x.CreateTime),
                     }).ToList();
    }

    /// <summary>
    /// 获取小节列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="chapterId">章节Id</param>
    /// <returns></returns>
    public async Task<IReadOnlyList<SectionRecordViewModel>> GetSectionList(Guid categoryId, Guid chapterId)
    {
        var key = $"userId_{_userId}:categoryId_{categoryId}:chapterId_{chapterId}";
        var cacheKey = GirvsEntityCacheDefaults<ChapterAnswerRecord>.QueryCacheKey.Create(key);
        var list = await cacheManager.GetAsync(cacheKey, () => repository.GetSectionList(_userId, categoryId, chapterId));
        return mapper.Map<IReadOnlyList<SectionRecordViewModel>>(list)
                     .GroupBy(x => x.SectionId)
                     .Select(g => new SectionRecordViewModel
                     {
                         SectionId = g.Key,
                         QuestionCount = g.Sum(x => x.QuestionCount),
                         AnswerCount = g.Sum(x => x.AnswerCount),
                         CorrectCount = g.Sum(x => x.CorrectCount),
                         CreateTime = g.Max(x => x.CreateTime),
                     }).ToList();
    }

    /// <summary>
    /// 获取知识点列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="sectionId">小节Id</param>
    /// <returns></returns>
    public async Task<IReadOnlyList<KnowledgePointRecordViewModel>> GetKnowledgePointList(Guid categoryId, Guid sectionId)
    {
        var key = $"userId_{_userId}:categoryId_{categoryId}:sectionId_{sectionId}";
        var cacheKey = GirvsEntityCacheDefaults<ChapterAnswerRecord>.QueryCacheKey.Create(key);
        var list = await cacheManager.GetAsync(cacheKey, () => repository.GetKnowledgePointList(_userId, categoryId, sectionId));
        // var list = await repository.GetKnowledgePointList(_userId, categoryId, sectionId);
        return mapper.Map<IReadOnlyList<KnowledgePointRecordViewModel>>(list)
                     .GroupBy(x => x.KnowledgePointId)
                     .Select(g => new KnowledgePointRecordViewModel
                     {
                         KnowledgePointId = g.Key,
                         QuestionCount = g.Sum(x => x.QuestionCount),
                         AnswerCount = g.Sum(x => x.AnswerCount),
                         CorrectCount = g.Sum(x => x.CorrectCount),
                         CreateTime = g.Max(x => x.CreateTime),
                     }).ToList();
    }

    /// <summary>
    /// 查询题库首页章节试题统计
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    public Task<ChapterRecordStat> GetChapterRecordStat(Guid subjectId)
    {
        var key = $"userId_{_userId}:subjectId:{subjectId}";
        var cacheKey = GirvsEntityCacheDefaults<ChapterAnswerRecord>.QueryCacheKey.Create(key);
        return cacheManager.GetAsync(cacheKey, () => repository.GetChapterRecordStat(_userId, subjectId));
        // return repository.GetChapterRecordStat(_userId, subjectId);
    }
}

public record BaseRecordViewModel : IDto
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

    /// <summary>
    /// 答题时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}

[AutoMapFrom(typeof(ChapterAnswerRecord))]
public record ChapterRecordViewModel : BaseRecordViewModel
{
    /// <summary>
    /// 章节Id
    /// </summary>
    public Guid ChapterId { get; init; }
}

[AutoMapFrom(typeof(ChapterAnswerRecord))]
public record SectionRecordViewModel : BaseRecordViewModel
{
    /// <summary>
    /// 小节Id
    /// </summary>
    public Guid SectionId { get; init; }
}

[AutoMapFrom(typeof(ChapterAnswerRecord))]
public record KnowledgePointRecordViewModel : BaseRecordViewModel
{
    /// <summary>
    /// 知识点Id
    /// </summary>
    public Guid KnowledgePointId { get; init; }
}