using HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.Service;
using HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;
using HaoKao.UserAnswerRecordService.Application.Worker;
using HaoKao.UserAnswerRecordService.Domain.Extensions;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Application.Modules.AnswerRecordModule;

public interface IDataAnalysisService : IManager
{
    /// <summary>
    /// 获取做题能力分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<PracticeAbilityAnalysisViewModel> GetPracticeAbilityAnalysis(Guid subjectId, Guid? userId = null);

    /// <summary>
    /// 刷新做题能力分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    ValueTask RefreshPracticeAbilityAnalysis(Guid subjectId, Guid? userId = null);

    /// <summary>
    /// 获取练习状况分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<PracticeSituationAnalysisViewModel> GetPracticeSituationAnalysis(Guid subjectId, Guid? userId = null);

    /// <summary>
    /// 刷新练习状况分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    ValueTask RefreshPracticeSituationAnalysis(Guid subjectId, Guid? userId = null);
}

/// <summary>
/// 数据分析服务
/// </summary>
public class DataAnalysisService(
    IStaticCacheManager cacheManager,
    IBackgroundTaskQueue taskQueue
) : IDataAnalysisService
{
    #region 私有成员

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IBackgroundTaskQueue _taskQueue = taskQueue ?? throw new ArgumentNullException(nameof(taskQueue));
    private readonly UserAnswerCacheDefaults _userAnswerCacheDefaults = new();

    #endregion

    #region 做题能力分析

    /// <summary>
    /// 获取做题能力分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public async Task<PracticeAbilityAnalysisViewModel> GetPracticeAbilityAnalysis(Guid subjectId, Guid? userId = null)
    {
        userId ??= EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var cacheKey = _userAnswerCacheDefaults.Statistics(userId.Value).Create($"{nameof(GetPracticeAbilityAnalysis)}:{subjectId}");
        var result = await _cacheManager.GetAsync(cacheKey, () => Task.FromResult(new PracticeAbilityAnalysisViewModel()));
        cacheKey = _userAnswerCacheDefaults.Refresh(userId.Value).Create($"{nameof(GetPracticeAbilityAnalysis)}:{subjectId}");
        result.Refreshable = await _cacheManager.GetAsync(cacheKey, () => Task.FromResult(true));
        return result;
    }

    /// <summary>
    /// 刷新做题能力分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public ValueTask RefreshPracticeAbilityAnalysis(Guid subjectId, Guid? userId = null)
    {
        userId ??= EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var model = new BuildWorkItemModel
        {
            SubjectId = subjectId,
            UserId = userId.Value,
            TenantId = EngineContext.Current.ClaimManager.GetTenantId(),
            ResultCacheKey = _userAnswerCacheDefaults.Statistics(userId.Value).Create($"{nameof(GetPracticeAbilityAnalysis)}:{subjectId}", cacheTime: int.MaxValue),
            RefreshCacheKey = _userAnswerCacheDefaults.Refresh(userId.Value).Create($"{nameof(GetPracticeAbilityAnalysis)}:{subjectId}", cacheTime: int.MaxValue),
        };
        return _taskQueue.EnqueueAsync(async _ =>
        {
            try
            {
                await BuildPracticeAbilityAnalysisWorkStoredProcedure(model);
            }
            catch (Exception)
            {
                EngineContext.Current.Resolve<IStaticCacheManager>().RemoveAsync(model.RefreshCacheKey).Wait();
                throw;
            }
        });
    }

    /// <summary>
    /// 构建做题能力分析任务
    /// </summary>
    /// <param name="model"></param>
    private static async ValueTask BuildPracticeAbilityAnalysisWorkStoredProcedure(BuildWorkItemModel model)
    {
        var logger = EngineContext.Current.Resolve<ILogger<UserAnswerRecordAppService>>();
        logger.LogInformation("开始刷新用户做题能力分析, 缓存key: {CacheKeyPrefix}", model.ResultCacheKey.Prefixes);

        var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
        await cacheManager.SetAsync(model.RefreshCacheKey, false);

        EngineContext.Current.ClaimManager.SetFromDictionary(new Dictionary<string, string>
        {
            { GirvsIdentityClaimTypes.UserId, model.UserId.ToString() },
            { GirvsIdentityClaimTypes.TenantId, model.TenantId }
        });
        var repository = EngineContext.Current.Resolve<IUserAnswerRecordRepository>();
        var result = new PracticeAbilityAnalysisViewModel();

        var dt = await repository.QueryPracticeAbility(model.SubjectId);
        if (dt.Rows.Count > 0)
        {
            var abilities = new List<PracticeAbilitySpModel>(dt.Rows.Count);
            foreach (DataRow dr in dt.Rows)
            {
                abilities.Add(new()
                {
                    Numeracy = dr["Numeracy"].To<int>(),
                    CorrectNumeracy = dr["CorrectNumeracy"].To<int>(),
                    Judgment = dr["Judgment"].To<int>(),
                    CorrectJudgment = dr["CorrectJudgment"].To<int>(),
                    Analytical = dr["Analytical"].To<int>(),
                    CorrectAnalytical = dr["CorrectAnalytical"].To<int>(),
                    Understanding = dr["Understanding"].To<int>(),
                    CorrectUnderstanding = dr["CorrectUnderstanding"].To<int>(),
                    Memory = dr["Memory"].To<int>(),
                    CorrectMemory = dr["CorrectMemory"].To<int>(),
                    CreatorId = dr["CreatorId"].To<Guid>()
                });
            }
            var myAbilities = abilities.Where(x => x.CreatorId == model.UserId).ToList();

            result = new PracticeAbilityAnalysisViewModel
            {
                // 同期学员
                AverageAbility = new()
                {
                    { PracticeAbilityDimension.CorrectNumeracy, Percentage(abilities.Sum(x => x.CorrectNumeracy), abilities.Sum(x => x.Numeracy)) },
                    { PracticeAbilityDimension.CorrectMemory, Percentage(abilities.Sum(x => x.CorrectMemory), abilities.Sum(x => x.Memory)) },
                    { PracticeAbilityDimension.CorrectUnderstanding, Percentage(abilities.Sum(x => x.CorrectUnderstanding), abilities.Sum(x => x.Understanding)) },
                    { PracticeAbilityDimension.CorrectJudgment, Percentage(abilities.Sum(x => x.CorrectJudgment), abilities.Sum(x => x.Judgment)) },
                    { PracticeAbilityDimension.CorrectAnalytical, Percentage(abilities.Sum(x => x.CorrectAnalytical), abilities.Sum(x => x.Analytical)) },
                },

                // 当前用户
                MyAbility = new()
                {
                    { PracticeAbilityDimension.CorrectNumeracy, Percentage(myAbilities.Sum(x => x.CorrectNumeracy), myAbilities.Sum(x => x.Numeracy)) },
                    { PracticeAbilityDimension.CorrectMemory, Percentage(myAbilities.Sum(x => x.CorrectMemory), myAbilities.Sum(x => x.Memory)) },
                    { PracticeAbilityDimension.CorrectUnderstanding, Percentage(myAbilities.Sum(x => x.CorrectUnderstanding), myAbilities.Sum(x => x.Understanding)) },
                    { PracticeAbilityDimension.CorrectJudgment, Percentage(myAbilities.Sum(x => x.CorrectJudgment), myAbilities.Sum(x => x.Judgment)) },
                    { PracticeAbilityDimension.CorrectAnalytical, Percentage(myAbilities.Sum(x => x.CorrectAnalytical), myAbilities.Sum(x => x.Analytical)) },
                },
            };
        }

        // 练习速度统计
        dt = await repository.QueryPracticeSpeed(model.SubjectId);
        if (dt.Rows.Count > 0)
        {
            var list = new List<PracticeSpeedModel>(dt.Rows.Count);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new()
                {
                    ElapsedTime = dr["ElapsedTime"].To<long>(),
                    AnswerCount = dr["AnswerCount"].To<int>(),
                    CreatorId = dr["CreatorId"].To<Guid>()
                });
            }
            var speeds = CalcPracticeSpeed(list, model.UserId);
            result.AverageAbility.Add(PracticeAbilityDimension.PracticeSpeed, speeds[0]);
            result.MyAbility.Add(PracticeAbilityDimension.PracticeSpeed, speeds[1]);
        }

        result.RefreshTime = DateTime.Now;

        cacheManager.RemoveAsync(model.RefreshCacheKey).Wait();
        cacheManager.RemoveAsync(model.ResultCacheKey).Wait();
        cacheManager.SetAsync(model.ResultCacheKey, result).Wait();
    }

    /// <summary>
    /// 计算练习速度
    /// </summary>
    /// <param name="list"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    private static double[] CalcPracticeSpeed(IReadOnlyCollection<PracticeSpeedModel> list, Guid userId)
    {
        var elapsedTimeDict = list.GroupBy(x => x.CreatorId)
                                  .ToDictionary(
                                      g => g.Key,
                                      g => g.Where(x => x.AnswerCount > 0).Sum(x => x.ElapsedTime * 1.0 / x.AnswerCount)
                                  );
        var numbers = elapsedTimeDict.Values.OrderBy(x => x).ToList(); // 从大到小排列
        if (numbers.Count == 0) return [0.00, 0.00,];

        var front5Median = GetMedian(numbers, 0.05); // 前5%的中位数
        var back5Median = GetMedian(numbers, 0.95); // 后5%的中位数

        // 同期学员
        var sum = list.Sum(x => x.ElapsedTime) * 1.0 / list.Sum(x => x.AnswerCount);
        var averageSpeed = Math.Round(GetPercentage(sum, front5Median, back5Median), 2);

        // 当前用户
        var userSpeed = elapsedTimeDict.TryGetValue(userId, out var elapsedTime)
            ? Math.Round(GetPercentage(elapsedTime, front5Median, back5Median), 2)
            : 1;

        return [(1 - averageSpeed) * 100, (1 - userSpeed) * 100,];

        static double GetMedian(IReadOnlyCollection<double> sortedNumbers, double percentile)
        {
            var count = sortedNumbers.Count;
            var index = (int)(percentile * count);
            if (index < 1) return sortedNumbers.First();

            // 获取前5%或后5%的元素
            var percentileNumbers = percentile < 0.5 ? sortedNumbers.Take(index).ToList() : sortedNumbers.ToArray()[(index - 1)..].ToList();

            // 计算中位数
            var n = percentileNumbers.Count;
            if (n % 2 == 0)
            {
                return (percentileNumbers[n / 2 - 1] + percentileNumbers[n / 2]) / 2;
            }
            return percentileNumbers[n / 2];
        }

        static double GetPercentage(double number, double lowerBound, double upperBound)
        {
            if (Math.Abs(lowerBound - upperBound) < 0.000001)
            {
                return 1.0;
            }
            if (number <= lowerBound)
            {
                return 0.0;
            }
            if (number >= upperBound)
            {
                return 1.0;
            }
            return (number - lowerBound) / (upperBound - lowerBound);
        }
    }

    #endregion

    #region 练习状况分析

    /// <summary>
    /// 获取练习状况分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public async Task<PracticeSituationAnalysisViewModel> GetPracticeSituationAnalysis(Guid subjectId, Guid? userId = null)
    {
        userId ??= EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var cacheKey = _userAnswerCacheDefaults.Statistics(userId.Value).Create($"{nameof(GetPracticeSituationAnalysis)}:{subjectId}");
        var result = await _cacheManager.GetAsync(cacheKey, () => Task.FromResult(new PracticeSituationAnalysisViewModel()));
        cacheKey = _userAnswerCacheDefaults.Refresh(userId.Value).Create($"{nameof(GetPracticeSituationAnalysis)}:{subjectId}");
        result.Refreshable = await _cacheManager.GetAsync(cacheKey, () => Task.FromResult(true));
        return result;
    }

    /// <summary>
    /// 刷新练习状况分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public ValueTask RefreshPracticeSituationAnalysis(Guid subjectId, Guid? userId = null)
    {
        userId ??= EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var model = new BuildWorkItemModel
        {
            SubjectId = subjectId,
            UserId = userId.Value,
            TenantId = EngineContext.Current.ClaimManager.GetTenantId(),
            ResultCacheKey = _userAnswerCacheDefaults.Statistics(userId.Value).Create($"{nameof(GetPracticeSituationAnalysis)}:{subjectId}", cacheTime: int.MaxValue),
            RefreshCacheKey = _userAnswerCacheDefaults.Refresh(userId.Value).Create($"{nameof(GetPracticeSituationAnalysis)}:{subjectId}", cacheTime: int.MaxValue),
        };
        return _taskQueue.EnqueueAsync(async _ =>
        {
            try
            {
                await BuildPracticeSituationAnalysisWorkUsingStoredProcedure(model);
            }
            catch (Exception)
            {
                EngineContext.Current.Resolve<IStaticCacheManager>().RemoveAsync(model.RefreshCacheKey).Wait();
                throw;
            }
        });
    }

    /// <summary>
    /// 构建练习能力分析任务
    /// </summary>
    /// <param name="model"></param>
    private static async ValueTask BuildPracticeSituationAnalysisWorkUsingStoredProcedure(BuildWorkItemModel model)
    {
        var logger = EngineContext.Current.Resolve<ILogger<UserAnswerRecordAppService>>();
        logger.LogInformation("开始刷新用户练习状况分析, 缓存key: {CacheKeyPrefix}", model.ResultCacheKey.Prefixes);

        var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
        await cacheManager.SetAsync(model.RefreshCacheKey, false);

        /*
         * 统计每道题是否做过时，取并集。例如我提交了100次记录，任一一次做了题目A，那么题目A就算做过。
         * 统计每道题是否正确时，取最新记录。例如我提交的100次记录中题目B有对有错，只要最近一次做错了，那么题目B就是做错了。
         */

        EngineContext.Current.ClaimManager.SetFromDictionary(new Dictionary<string, string>
        {
            { GirvsIdentityClaimTypes.UserId, model.UserId.ToString() },
            { GirvsIdentityClaimTypes.TenantId, model.TenantId }
        });
        var repository = EngineContext.Current.Resolve<IUserAnswerRecordRepository>();

        var dt = await repository.QueryPracticeSituation(model.SubjectId);
        var row = dt.Rows[0];
        var result = new PracticeSituationAnalysisViewModel
        {
            AnswerCount = row[0].To<int>(),
            CorrectCount = row[1].To<int>(),
            WrongCount = row[2].To<int>(),
            MissedCount = row[3].To<int>(),
            TotalAnswerCount = row[4].To<int>(),
            TotalCorrectCount = row[5].To<int>(),
            QuestionCount = row[6].To<int>(),
            AverageAnswerCount = row[7].To<int>(),
            QuestionTypeAccuracy = []
        };

        result.MyProgress = Percentage(result.TotalAnswerCount, result.QuestionCount);
        result.AverageProgress = Percentage(result.AverageAnswerCount, result.QuestionCount);

        // 计算准确率和排名
        dt = await repository.QueryAccuracyRank(model.SubjectId);
        var linq = from DataRow dr in dt.Rows
                   let x = dr[0].To<int>()
                   let y = dr[1].To<int>()
                   select Percentage(x, y);
        var list = linq.OrderByDescending(x => x).ToList();

        result.MyAccuracy = Percentage(result.CorrectCount, result.AnswerCount);
        result.AverageAccuracy = Math.Round(list.Sum() / list.Count, 2);

        // 计算当前用户的准确率排名
        // 如果 result.MyAccuracy 不在 list 中。
        // 这可能会导致 rank 为 0，从而导致 Percentage(rank, list.Count) 返回 0
        // 为了避免这种情况，需要计算 rank 在 list 中的位置
        var rank = 1;
        foreach (var i in list)
        {
            if (result.MyAccuracy < i)
            {
                rank++;
            }
        }
        result.MyAccuracyPercentileRank = Percentage(rank, list.Count);
        if (result.MyAccuracyPercentileRank <= 0) result.MyAccuracyPercentileRank = 1;
        if (result.MyAccuracyPercentileRank >= 100) result.MyAccuracyPercentileRank = 99;

        // 计算不同题型的准确率
        dt = await repository.QueryQuestionTypeAccuracy(model.SubjectId);
        if (dt.Rows.Count > 0)
        {
            var dict = new Dictionary<Guid, double>(dt.Rows.Count);
            var dict2 = new Dictionary<Guid, double>(dt.Rows.Count);
            foreach (DataRow dr in dt.Rows)
            {
                var correctCount = dr[0].To<int>();
                var answerCount = dr[1].To<int>();
                var questionTypeId = dr[2].To<Guid>();
                var isCaseAnalysis = dr[3].To<int>() == 1;
                var userId = dr[4].To<Guid>();

                var key = isCaseAnalysis ? new Guid("68eae47e-3df7-4c78-9e73-0294bcbdd7ac") : questionTypeId;
                var value = Percentage(correctCount, answerCount);

                if (model.UserId == userId)
                {
                    if (!dict.TryAdd(key, value))
                    {
                        dict[key] += value;
                    }
                }

                if (!dict2.TryAdd(key, value))
                {
                    dict2[key] += value;
                }
            }

            result.QuestionTypeAccuracy = dict;
            result.AverageQuestionTypeAccuracy = dict2;

            result.RefreshTime = DateTime.Now;
            cacheManager.RemoveAsync(model.RefreshCacheKey).Wait();
            cacheManager.RemoveAsync(model.ResultCacheKey).Wait();
            cacheManager.SetAsync(model.ResultCacheKey, result).Wait();
        }
    }

    /// <summary>
    /// 计算百分比
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private static double Percentage(int x, int y)
    {
        return Math.Round(y == 0 ? 0 : 100.0 * x / y, 2);
    }

    #endregion
}