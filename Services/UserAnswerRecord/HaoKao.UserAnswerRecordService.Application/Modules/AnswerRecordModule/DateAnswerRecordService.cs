using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Application.Modules.AnswerRecordModule;

public interface IDateAnswerRecordService : IManager
{
    Task<AnswerRecordViewModel> Get(Guid id);

    Task<Guid> GetTodayTaskRecordId(Guid subjectId);

    Task<IReadOnlyList<DateOnly>> GetDailyRecordList(Guid subjectId, DateOnly date);

    Task<decimal> GetDailyRankingRatio(Guid subjectId, DateOnly date);

    Task<int> GetDailyUserCount(Guid subjectId);
}

/// <summary>
/// 打卡和今日任务答题记录服务
/// </summary>
/// <param name="mapper"></param>
/// <param name="cacheManager"></param>
/// <param name="repository"></param>
public class DateAnswerRecordService(
    IMapper mapper,
    IStaticCacheManager cacheManager,
    IDateAnswerRecordRepository repository
) : IDateAnswerRecordService
{
    private readonly Guid _userId = EngineContext.Current.IsAuthenticated
        ? EngineContext.Current.ClaimManager.GetUserId().To<Guid>()
        : Guid.Empty;

    /// <summary>
    /// 按Id获取记录详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    public async Task<AnswerRecordViewModel> Get(Guid id)
    {
        var cacheKey = GirvsEntityCacheDefaults<DateAnswerRecord>.ByIdCacheKey.Create(id.ToString());
        var entity = await cacheManager.GetAsync(cacheKey, () => repository.GetById(id));
        // var entity = await repository.GetById(id);
        return mapper.Map<AnswerRecordViewModel>(entity.AnswerRecord);
    }

    /// <summary>
    /// 获取今日任务作答记录Id
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    public async Task<Guid> GetTodayTaskRecordId(Guid subjectId)
    {
        var key = $"TodayTaskRecordId:userId_{_userId}:subjectId_{subjectId}";
        var cacheKey = GirvsEntityCacheDefaults<DateAnswerRecord>.ByIdCacheKey.Create(key);
        var entity = await cacheManager.GetAsync(cacheKey, () => repository.GetTodayTaskRecord(_userId, subjectId));
        return entity?.Id ?? Guid.Empty;
    }

    /// <summary>
    /// 获取每日打卡记录(月)
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <param name="date">日期</param>
    /// <returns></returns>
    public Task<IReadOnlyList<DateOnly>> GetDailyRecordList(Guid subjectId, DateOnly date)
    {
        var key = $"DailyRecordList:userId_{_userId}:subjectId_{subjectId}:date_{date}";
        var cacheKey = GirvsEntityCacheDefaults<DateAnswerRecord>.QueryCacheKey.Create(key);
        return cacheManager.GetAsync(cacheKey, () => repository.GetDailyRecordList(_userId, subjectId, date));
    }

    /// <summary>
    /// 获取每日打卡排名占比(月)
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <param name="date">日期</param>
    /// <returns></returns>
    public async Task<decimal> GetDailyRankingRatio(Guid subjectId, DateOnly date)
    {
        var key = $"DailyRankingRatio:userId_{_userId}:subjectId_{subjectId}:date_{date}";
        var cacheKey = GirvsEntityCacheDefaults<DateAnswerRecord>.QueryCacheKey.Create(key);
        return await cacheManager.GetAsync(cacheKey, () => repository.GetDailyRankingRatio(_userId, subjectId, date));
    }

    /// <summary>
    /// 每日一题打卡人数统计(每日00:00更新)
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    public Task<int> GetDailyUserCount(Guid subjectId)
    {
        var cacheTime = TimeSpan.FromDays(1).Minutes;
        var key = $"DailyUserCount:{subjectId}-{DateTime.Now:yyyy-MM-dd}";
        var cacheKey = GirvsEntityCacheDefaults<DateAnswerRecord>.ByTenantKey.Create(key, cacheTime: cacheTime);
        return cacheManager.GetAsync(cacheKey, () => repository.GetDailyUserCount(subjectId));
    }
}