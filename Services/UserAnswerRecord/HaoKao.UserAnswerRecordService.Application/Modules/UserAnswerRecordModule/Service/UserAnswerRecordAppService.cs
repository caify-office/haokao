using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.Interfaces;
using HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;
using HaoKao.UserAnswerRecordService.Application.Worker;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Extensions;
using HaoKao.UserAnswerRecordService.Domain.Queries;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.Service;

/// <summary>
/// 用户答题记录服务--App
/// </summary>
// [DynamicWebApi(Module = ServiceAreaName.App)]
// [Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class UserAnswerRecordAppService(
    IStaticCacheManager cacheManager,
    IUserAnswerRecordRepository repository,
    IBackgroundTaskQueue taskQueue,
    IMapper mapper
) : IUserAnswerRecordAppService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IUserAnswerRecordRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IBackgroundTaskQueue _taskQueue = taskQueue ?? throw new ArgumentNullException(nameof(taskQueue));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly UserAnswerCacheDefaults _userAnswerCacheDefaults = new();

    #region 服务方法

    /// <summary>
    /// 按Id获取记录(答题结果页面)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<UserAnswerRecordAppViewModel> Get(Guid id)
    {
        var record = await _cacheManager.GetAsync(
            _userAnswerCacheDefaults.ById.Create(id.ToString()),
            () => _repository.GetByIdIncludeDetail(id)
        );
        return _mapper.Map<UserAnswerRecordAppViewModel>(record);
    }

    /// <summary>
    /// 根据查询条件获取指定的答题记录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<UserAnswerRecordQueryViewModel> Get([FromQuery] UserAnswerRecordQueryViewModel input)
    {
        var query = input.MapToQuery<UserAnswerRecordQuery>();
        query.OrderBy = $"{nameof(UserAnswerRecord.CreateTime)}";

        var tempQuery = await _cacheManager.GetAsync(
            _userAnswerCacheDefaults.ListQuery.Create(query.GetCacheKey()),
            async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            }
        );

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<UserAnswerRecordQueryViewModel, UserAnswerRecord>();
    }

    /// <summary>
    /// 获取章节列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<UserAnswerRecordChapterListAppViewModel>> GetChapterList
        ([FromQuery] UserAnswerRecordAppQueryModel input)
    {
        var queryModel = _mapper.Map<UserAnswerRecordQuery>(input);
        queryModel.AnswerType = SubmitAnswerType.Chapter;
        var queryFields = typeof(UserAnswerRecordChapterListAppViewModel).GetTypeQueryFields();
        var list = await _cacheManager.GetAsync(
            _userAnswerCacheDefaults.ListQuery.Create(queryModel.GetCacheKey()),
            () => _repository.GetLatestRecordListAsync(queryModel.GetQueryWhere(), queryFields)
        );
        return _mapper.Map<List<UserAnswerRecordChapterListAppViewModel>>(list);
    }

    /// <summary>
    /// 获取试卷列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<UserAnswerRecordPaperListAppViewModel>> GetPaperList
        ([FromQuery] UserAnswerRecordAppQueryModel input)
    {
        var queryModel = _mapper.Map<UserAnswerRecordQuery>(input);
        queryModel.AnswerType = SubmitAnswerType.Paper;
        var queryFields = typeof(UserAnswerRecordPaperListAppViewModel).GetTypeQueryFields();
        var list = await _cacheManager.GetAsync(
            _userAnswerCacheDefaults.ListQuery.Create(queryModel.GetCacheKey()),
            () => _repository.GetLatestRecordListAsync(queryModel.GetQueryWhere(), queryFields)
        );
        // var list = await _repository.GetLatestRecordListAsync(queryModel.GetQueryWhere(), queryFields);
        return _mapper.Map<List<UserAnswerRecordPaperListAppViewModel>>(list);
    }

    /// <summary>
    /// 查询用户最新的练习记录Id
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<Guid> GetUserLatestRecordId()
    {
        // 根据当前用户id读取用户 提交的最新一次的作答记录id
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return _cacheManager.GetAsync(
            _userAnswerCacheDefaults.ListQuery.Create(nameof(GetUserLatestRecordId)),
            () => _repository.GetUserLatestRecordId(userId)
        );
    }

    /// <summary>
    /// 查询今日任务的练习记录Id
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<Guid> GetTodayTaskRecordId(Guid subjectId)
    {
        // cacheTime 单位是分钟
        var cacheTime = (DateTime.Today.AddDays(1) - DateTime.Now).Minutes;
        var cacheKey = $"{nameof(GetTodayTaskRecordId)}:{subjectId}{DateTime.Now:yyyy-MM-dd}";
        return _cacheManager.GetAsync(
            _userAnswerCacheDefaults.ListQuery.Create(cacheKey, cacheTime: cacheTime),
            () => _repository.GetTodayTaskRecordId(subjectId)
        );
    }

    #endregion

    #region 题库首页

    /// <summary>
    /// 查询题库首页章节试题统计
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public async Task<ChapterRecordStatViewModel> GetChapterRecordStat(Guid subjectId)
    {
        var key = _userAnswerCacheDefaults.ListOther.Create(subjectId.ToString());
        var model = await _cacheManager.GetAsync(key, async () =>
        {
            var table = await _repository.GetChapterRecordStat(subjectId);
            if (table.Rows.Count == 0) return new ChapterRecordStatViewModel();

            var dr = table.Rows[0];
            var chapterAnswerCount = ParseToInt(dr["ChapterAnswerCount"]);
            var answerCount = ParseToInt(dr["AnswerCount"]);
            var correctCount = ParseToInt(dr["CorrectCount"]);
            var elapsedTime = ParseToInt(dr["ElapsedTime"]);

            var accuracy = Percentage(correctCount, answerCount); //正确率
            var questionTime = elapsedTime > 0 && answerCount > 0 ? elapsedTime / answerCount : 0; //每题耗时

            return new ChapterRecordStatViewModel
            {
                AnswerCount = answerCount,
                ChapterCount = chapterAnswerCount,
                QuestionTime = questionTime,
                Accuracy = Math.Round(accuracy, 2),
            };
        });

        return model;

        int ParseToInt(object value)
        {
            return int.TryParse(value.ToString(), out var number) ? number : 0;
        }
    }

    /// <summary>
    /// 查询题库首页试卷详细情况
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<PaperAnswerDetailAppViewModel>> GetUserPaperDetail
        ([FromBody] UserAnswerRecordAppQueryModel input)
    {
        var query = _mapper.Map<UserAnswerRecordQuery>(input);
        var queryFields = typeof(PaperAnswerDetailQueryFieldModel).GetTypeQueryFields();

        var list = await _cacheManager.GetAsync(
            _userAnswerCacheDefaults.ListQuery.Create(query.GetCacheKey()),
            () => _repository.GetWhereAsync(query.GetQueryWhere(), queryFields)
        );

        if (list.Count == 0) return [];

        var dto = _mapper.Map<List<PaperAnswerDetailAppViewModel>>(list);
        //更新频率太快，暂时不做缓存
        var dict = await _repository.GetPaperAnswerCountDict(input.RecordIdentifier);
        foreach (var item in dto)
        {
            if (!dict.TryGetValue(item.RecordIdentifier, out var count))
            {
                item.FinishCount = count;
            }
        }

        return dto;
    }

    /// <summary>
    /// 题库首页整卷测验统计接口以及首页点击进去列表查询接口
    /// </summary>
    /// <param name="paperIds"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<AppHomePaperAnswerViewModel>> GetHomePaperAnswerDetail(Guid[] paperIds)
    {
        //获取当前用户每个试卷的得分
        //var userScore = await _repository.GetCurrentUserPaperScore(paperIds);
        var userScore = await _cacheManager.GetAsync(
            _userAnswerCacheDefaults.ListQuery.Create($"{nameof(GetHomePaperAnswerDetail)}:{string.Join("-", paperIds).ToMd5()}"),
            () => _repository.GetCurrentUserPaperScore(paperIds)
        );

        //获取试卷的回答的总人数(统计高频刷新，暂时不做缓存)
        var participateCount = await _repository.GetPaperUserAnswerRecordCount(paperIds);

        return paperIds.Select(x =>
        {
            var result = new AppHomePaperAnswerViewModel
            {
                PaperId = x,
                ExamNumber = participateCount.TryGetValue(x, out var value) ? value : 0,
            };

            var userPaperScore = userScore.FirstOrDefault(s => s.RecordIdentifier == x);
            if (userPaperScore != null)
            {
                result.Progress = userPaperScore.AnswerCount / userPaperScore.QuestionCount;
                result.CurrentScore = userPaperScore.UserScore;
            }

            return result;
        }).ToList();
    }

    #endregion

    #region 每日一题

    /// <summary>
    /// 查询当月每日一题打卡记录
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}/{dateTime:datetime}")]
    public Task<List<DateTime>> GetPunchInRecordList(Guid subjectId, DateTime dateTime)
    {
        var date = new DateTime(dateTime.Year, dateTime.Month, 1);
        var query = new UserAnswerRecordQuery
        {
            SubjectId = subjectId,
            StartDateTime = date,
            EndDateTime = date.AddMonths(1),
            AnswerType = SubmitAnswerType.Daily,
        };

        return _cacheManager.GetAsync(
            _userAnswerCacheDefaults.ListQuery.Create(query.GetCacheKey()),
            () => _repository.GetPunchInRecordList(subjectId, DateOnly.FromDateTime(date))
        );
        // return _repository.GetPunchInRecordList(subjectId, DateOnly.FromDateTime(date));
    }

    /// <summary>
    /// 获取打卡排名比例
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}/{dateTime:datetime}")]
    public async Task<decimal> GetPunchInRankingRatio(Guid subjectId, DateTime dateTime)
    {
        var date = new DateTime(dateTime.Year, dateTime.Month, 1);
        var query = new UserAnswerRecordQuery
        {
            SubjectId = subjectId,
            StartDateTime = date,
            EndDateTime = date.AddMonths(1),
            AnswerType = SubmitAnswerType.Daily,
            UserId = null,
        };

        var result = await _repository.GetPunchInRankingRatio(query.GetQueryWhere());

        return Convert.ToDecimal(result);
    }

    /// <summary>
    /// 每日一题打卡人数统计(每日00:00更新)
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<int> GetJoinPunchInPeopleNumber(Guid subjectId)
    {
        var date = DateTime.Now.Date;
        var keyStr = $"{subjectId}-{date}".ToMd5();
        var key = _userAnswerCacheDefaults.ListQuery.Create(keyStr);
        key.CacheTime = TimeSpan.FromDays(1).Minutes;

        return _cacheManager.GetAsync(
            key, () => _repository.GetJoinPunchInPeopleNumber(subjectId)
        );
    }

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